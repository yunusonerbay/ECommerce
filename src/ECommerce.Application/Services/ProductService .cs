using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace ECommerce.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IBalanceManagementService _balanceService;
        private readonly ILogger<ProductService> _logger;

        public ProductService(IBalanceManagementService balanceService,ILogger<ProductService> logger)
        {
            _balanceService = balanceService;
            _logger = logger;
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            _logger.LogInformation("Retrieving all products from balance management service");
            var products = await _balanceService.GetProductsAsync();

            return products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                Currency = p.Currency,
                Category = p.Category,
                Stock = p.Stock
            });
        }

        public async Task<ProductDto?> GetProductByIdAsync(string id)
        {
            _logger.LogInformation("Retrieving product with ID: {ProductId}", id);
            var products = await _balanceService.GetProductsAsync();
            var product = products.FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                _logger.LogWarning("Product with ID: {ProductId} not found", id);
                return null;
            }

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Currency = product.Currency,
                Category = product.Category,
                Stock = product.Stock
            };
        }
    }
}