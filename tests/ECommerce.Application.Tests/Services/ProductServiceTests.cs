using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.Application.DTOs;
using ECommerce.Application.ExternalServices;
using ECommerce.Application.Services;
using ECommerce.Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace ECommerce.Application.Tests.Services
{
    public class ProductServiceTests
    {
        private readonly Mock<IBalanceManagementService> _mockBalanceService;
        private readonly Mock<ILogger<ProductService>> _mockLogger;
        private readonly ProductService _productService;

        public ProductServiceTests()
        {
            _mockBalanceService = new Mock<IBalanceManagementService>();
            _mockLogger = new Mock<ILogger<ProductService>>();
            _productService = new ProductService(_mockBalanceService.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetAllProductsAsync_ShouldReturnAllProducts()
        {
 
            var products = new List<Product>
            {
                new Product { Id = "prod-1", Name = "Product 1", Description = "Description 1", Price = 10.99m, Currency = "USD", Category = "Category 1", Stock = 100 },
                new Product { Id = "prod-2", Name = "Product 2", Description = "Description 2", Price = 20.99m, Currency = "USD", Category = "Category 2", Stock = 50 }
            };

            _mockBalanceService.Setup(m => m.GetProductsAsync())
                .ReturnsAsync(products);

            var result = await _productService.GetAllProductsAsync();

            Assert.Equal(2, result.Count());
            Assert.Equal("Product 1", result.First().Name);
            Assert.Equal(10.99m, result.First().Price);

            _mockBalanceService.Verify(m => m.GetProductsAsync(), Times.Once);
        }

        [Fact]
        public async Task GetProductByIdAsync_WithValidId_ShouldReturnProduct()
        {
            var products = new List<Product>
            {
                new Product { Id = "prod-1", Name = "Product 1", Description = "Description 1", Price = 10.99m, Currency = "USD", Category = "Category 1", Stock = 100 },
                new Product { Id = "prod-2", Name = "Product 2", Description = "Description 2", Price = 20.99m, Currency = "USD", Category = "Category 2", Stock = 50 }
            };

            _mockBalanceService.Setup(m => m.GetProductsAsync())
                .ReturnsAsync(products);

            var result = await _productService.GetProductByIdAsync("prod-1");

            Assert.NotNull(result);
            Assert.Equal("Product 1", result.Name);
            Assert.Equal(10.99m, result.Price);

            _mockBalanceService.Verify(m => m.GetProductsAsync(), Times.Once);
        }

        [Fact]
        public async Task GetProductByIdAsync_WithInvalidId_ShouldReturnNull()
        {
            var products = new List<Product>
            {
                new Product { Id = "prod-1", Name = "Product 1", Description = "Description 1", Price = 10.99m, Currency = "USD", Category = "Category 1", Stock = 100 },
                new Product { Id = "prod-2", Name = "Product 2", Description = "Description 2", Price = 20.99m, Currency = "USD", Category = "Category 2", Stock = 50 }
            };

            _mockBalanceService.Setup(m => m.GetProductsAsync())
                .ReturnsAsync(products);

            var result = await _productService.GetProductByIdAsync("non-existent-id");

            Assert.Null(result);

            _mockBalanceService.Verify(m => m.GetProductsAsync(), Times.Once);
        }
    }
}