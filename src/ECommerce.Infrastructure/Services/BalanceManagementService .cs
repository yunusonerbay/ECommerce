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
using ECommerce.Infrastructure.Models;
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

        public async Task<string> CreatePreorderAsync(string buyerId, List<OrderItem> items, decimal totalAmount)
        {
            try
            {
                _logger.LogInformation("Creating preorder with Balance Management API for buyer {BuyerId}", buyerId);

                var requestData = new
                {
                    BuyerId = buyerId,
                    Items = items.Select(i => new
                    {
                        ProductId = i.ProductId,
                        Quantity = i.Quantity,
                        UnitPrice = i.UnitPrice
                    }).ToList(),
                    TotalAmount = totalAmount
                };

                var content = new StringContent(
                    JsonSerializer.Serialize(requestData, _jsonOptions),
                    Encoding.UTF8,
                    "application/json");

                var response = await _httpClient.PostAsync("/api/preorder", content);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                var responseObject = JsonSerializer.Deserialize<PreorderResponse>(responseContent, _jsonOptions);

                if (responseObject?.Success != true || responseObject.Data == null)
                {
                    throw new PaymentFailedException("API returned unsuccessful response or null data");
                }

                return responseObject.Data.TransactionId;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error creating preorder with Balance Management API");
                throw new PaymentFailedException($"Failed to create preorder: {ex.Message}");
            }
        }

        public async Task CompleteOrderAsync(string transactionId)
        {
            try
            {
                _logger.LogInformation("Completing order with Balance Management API, transaction ID: {TransactionId}", transactionId);

                var requestData = new
                {
                    TransactionId = transactionId
                };

                var content = new StringContent(
                    JsonSerializer.Serialize(requestData, _jsonOptions),
                    Encoding.UTF8,
                    "application/json");

                var response = await _httpClient.PostAsync("/api/complete", content);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                var responseObject = JsonSerializer.Deserialize<BaseResponse>(responseContent, _jsonOptions);

                if (responseObject?.Success != true)
                {
                    throw new PaymentFailedException("API returned unsuccessful response");
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error completing order with Balance Management API");
                throw new PaymentFailedException($"Failed to complete payment: {ex.Message}");
            }
        }

    }
}