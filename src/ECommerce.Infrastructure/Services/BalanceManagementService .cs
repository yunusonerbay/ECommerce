using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ECommerce.Application.ExternalServices;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Exceptions;
using ECommerce.Infrastructure.Models.ApiResponses;
using Microsoft.Extensions.Logging;

namespace ECommerce.Infrastructure.Services
{
    public class BalanceManagementService : IBalanceManagementService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<BalanceManagementService> _logger;
        private readonly JsonSerializerOptions _jsonOptions;

        public BalanceManagementService(HttpClient httpClient, ILogger<BalanceManagementService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            try
            {
                _logger.LogInformation("Fetching products from Balance Management API");

                var response = await _httpClient.GetAsync("/api/products");
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                var responseObject = JsonSerializer.Deserialize<ProductsResponse>(responseContent, _jsonOptions);

                if (responseObject?.Success != true || responseObject.Data == null)
                {
                    _logger.LogWarning("API returned unsuccessful response or null data");
                    return new List<Product>();
                }

                return responseObject.Data;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error fetching products from Balance Management API");
                throw new DomainException($"Failed to retrieve products: {ex.Message}");
            }
        }

        public async Task<Balance> GetBalanceAsync()
        {
            try
            {
                _logger.LogInformation("Fetching balance information");

                var response = await _httpClient.GetAsync("/api/balance");
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                var responseObject = JsonSerializer.Deserialize<BalanceResponse>(responseContent, _jsonOptions);

                if (responseObject?.Success != true || responseObject.Data == null)
                {
                    _logger.LogWarning("API returned unsuccessful response or null data");
                    throw new DomainException("Failed to retrieve balance information");
                }

                return responseObject.Data;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error fetching balance from Balance Management API");
                throw new DomainException($"Failed to retrieve balance: {ex.Message}");
            }
        }

        public async Task<bool> CreatePreorderAsync(string orderId, decimal amount)
        {
            try
            {
                _logger.LogInformation("Creating preorder with Balance Management API for order {OrderId}, amount: {Amount}", orderId, amount);

                var requestData = new
                {
                    orderId = orderId,
                    amount = amount
                };

                var content = new StringContent(
                    JsonSerializer.Serialize(requestData, _jsonOptions),
                    Encoding.UTF8,
                    "application/json");

                var response = await _httpClient.PostAsync("/api/balance/preorder", content);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                var responseObject = JsonSerializer.Deserialize<PreorderResponse>(responseContent, _jsonOptions);

                if (responseObject?.Success != true || responseObject.Data == null)
                {
                    _logger.LogWarning("API returned unsuccessful response or null data");
                    throw new PaymentFailedException("API returned unsuccessful response or null data");
                }

                _logger.LogInformation("Preorder created successfully for order {OrderId}, status: {Status}",
                    responseObject.Data.PreOrder?.OrderId, responseObject.Data.PreOrder?.Status);

                return true;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error creating preorder with Balance Management API");
                throw new PaymentFailedException($"Failed to create preorder: {ex.Message}");
            }
        }

        public async Task<bool> CompleteOrderAsync(string orderId)
        {
            try
            {
                _logger.LogInformation("Completing order with Balance Management API, order ID: {OrderId}", orderId);

                var requestData = new
                {
                    orderId = orderId
                };

                var content = new StringContent(
                    JsonSerializer.Serialize(requestData, _jsonOptions),
                    Encoding.UTF8,
                    "application/json");

                var response = await _httpClient.PostAsync("/api/balance/complete", content);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                var responseObject = JsonSerializer.Deserialize<CompleteOrderResponse>(responseContent, _jsonOptions);

                if (responseObject?.Success != true || responseObject.Data == null)
                {
                    _logger.LogWarning("API returned unsuccessful response or null data");
                    throw new PaymentFailedException("API returned unsuccessful response or null data");
                }

                _logger.LogInformation("Order completed successfully: {OrderId}, status: {Status}",
                    responseObject.Data.Order?.OrderId, responseObject.Data.Order?.Status);

                return true;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error completing order with Balance Management API");
                throw new PaymentFailedException($"Failed to complete payment: {ex.Message}");
            }
        }

    }
}