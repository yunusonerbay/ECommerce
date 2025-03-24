using System.Collections.Generic;
using System.Threading.Tasks;
using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ECommerce.WebApi.Controllers
{
    /// <summary>
    /// Orders API - Manages order creation, completion and retrieval
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(IOrderService orderService, ILogger<OrdersController> logger)
        {
            _orderService = orderService;
            _logger = logger;
        }

        /// <summary>
        /// Create a new order with product list and reserve funds
        /// </summary>
        /// <param name="createOrderDto">Order creation data</param>
        /// <returns>Created order details</returns>
        [HttpPost("create")]
        [ProducesResponseType(typeof(OrderDto), 201)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public async Task<ActionResult<OrderDto>> CreateOrder([FromBody] CreateOrderDto createOrderDto)
        {
            _logger.LogInformation("Creating new order for buyer: {BuyerId}", createOrderDto.BuyerId);
            var order = await _orderService.CreateOrderAsync(createOrderDto);
            return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
        }

        /// <summary>
        /// Complete an order and finalize payment
        /// </summary>
        /// <param name="id">Order Id</param>
        /// <returns>Completed order details</returns>
        [HttpPost("{id}/complete")]
        [ProducesResponseType(typeof(OrderDto), 200)]
        [ProducesResponseType(typeof(ProblemDetails), 400)]
        [ProducesResponseType(typeof(ProblemDetails), 404)]
        [ProducesResponseType(typeof(ProblemDetails), 500)]
        public async Task<ActionResult<OrderDto>> CompleteOrder(string id)
        {
            _logger.LogInformation("Completing order with ID: {OrderId}", id);
            var order = await _orderService.CompleteOrderAsync(id);
            return Ok(order);
        }

        /// <summary>
        /// Get an order by Id
        /// </summary>
        /// <param name="id">Order Id</param>
        /// <returns>Order details</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(OrderDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<OrderDto>> GetOrder(string id)
        {
            _logger.LogInformation("Getting order with ID: {OrderId}", id);
            var order = await _orderService.GetOrderByIdAsync(id);

            if (order == null)
                return NotFound();

            return Ok(order);
        }

        /// <summary>
        /// Get all orders for a buyer
        /// </summary>
        /// <param name="buyerId">Buyer Id</param>
        /// <returns>List of orders</returns>
        [HttpGet("buyer/{buyerId}")]
        [ProducesResponseType(typeof(IEnumerable<OrderDto>), 200)]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrdersByBuyer(string buyerId)
        {
            _logger.LogInformation("Getting orders for buyer: {BuyerId}", buyerId);
            var orders = await _orderService.GetOrdersByBuyerIdAsync(buyerId);
            return Ok(orders);
        }
    }
}