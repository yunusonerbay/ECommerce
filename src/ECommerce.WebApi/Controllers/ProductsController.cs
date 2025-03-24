using System.Collections.Generic;
using System.Threading.Tasks;
using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ECommerce.WebApi.Controllers
{
    /// <summary>
    /// Products API - Manages product information and availability
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IProductService productService, ILogger<ProductsController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        /// <summary>
        /// Get all available products with pricing
        /// </summary>
        /// <returns>List of products</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ProductDto>), 200)]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
        {
            _logger.LogInformation("Getting all products");
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }

        /// <summary>
        /// Get product by Id
        /// </summary>
        /// <param name="id">Product Id</param>
        /// <returns>Product details</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ProductDto>> GetProduct(string id)
        {
            _logger.LogInformation("Getting product with ID: {ProductId}", id);
            var product = await _productService.GetProductByIdAsync(id);

            if (product == null)
                return NotFound();

            return Ok(product);
        }
    }
}