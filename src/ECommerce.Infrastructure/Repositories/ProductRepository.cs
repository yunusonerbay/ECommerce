using System.Collections.Generic;
using System.Threading.Tasks;
using ECommerce.Application.ExternalServices;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace ECommerce.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IBalanceManagementService _balanceService;
        private readonly ILogger<ProductRepository> _logger;

        public ProductRepository(IBalanceManagementService balanceService, ILogger<ProductRepository> logger)
        {
            _balanceService = balanceService;
            _logger = logger;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            _logger.LogInformation("Getting all products from Balance Management API");
            return await _balanceService.GetProductsAsync();
        }

        public async Task<Product?> GetByIdAsync(string id)
        {
            _logger.LogInformation("Getting product with ID: {ProductId}", id);
            var products = await _balanceService.GetProductsAsync();
            return products.FirstOrDefault(p => p.Id == id);
        }

        public Task UpdateStockAsync(string id, int quantity)
        {
            // In this implementation, stock is managed by the Balance Management API
            // We don't need to update it locally
            _logger.LogInformation("Stock update for product with ID: {ProductId} was requested, but stock is managed by Balance Management API", id);
            return Task.CompletedTask;
        }
    }
}