using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.Application.DTOs;
using ECommerce.Application.ExternalServices;
using ECommerce.Application.Interfaces;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Enums;
using ECommerce.Domain.Exceptions;
using ECommerce.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace ECommerce.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductService _productService;
        private readonly IBalanceManagementService _balanceService;
        private readonly ILogger<OrderService> _logger;

        public OrderService(
            IOrderRepository orderRepository,
            IProductService productService,
            IBalanceManagementService balanceService,
            ILogger<OrderService> logger)
        {
            _orderRepository = orderRepository;
            _productService = productService;
            _balanceService = balanceService;
            _logger = logger;
        }

        public async Task<OrderDto> CreateOrderAsync(CreateOrderDto createOrderDto)
        {
            _logger.LogInformation("Creating new order for buyer: {BuyerId}", createOrderDto.BuyerId);

            // Validate products and calculate total amount
            var orderItems = new List<OrderItem>();
            decimal totalAmount = 0;

            foreach (var item in createOrderDto.Items)
            {
                var product = await _productService.GetProductByIdAsync(item.ProductId);

                if (product == null)
                {
                    _logger.LogWarning("Product with ID {ProductId} not found", item.ProductId);
                    throw new DomainException($"Product with ID {item.ProductId} not found");
                }

                if (product.Stock < item.Quantity)
                {
                    _logger.LogWarning("Insufficient stock for product {ProductId}. Requested: {Requested}, Available: {Available}",
                        product.Id, item.Quantity, product.Stock);
                    throw new InsufficientStockException(product.Id, item.Quantity, product.Stock);
                }

                var orderItem = new OrderItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Quantity = item.Quantity,
                    UnitPrice = product.Price
                };

                orderItems.Add(orderItem);
                totalAmount += orderItem.UnitPrice * orderItem.Quantity;
            }

            // Create a unique order ID
            var orderId = Guid.NewGuid().ToString();

            // Try to reserve funds through the balance service
            try
            {
                _logger.LogInformation("Reserving funds for order {OrderId}, amount: {Amount}", orderId, totalAmount);

                var success = await _balanceService.CreatePreorderAsync(orderId, totalAmount);

                if (!success)
                {
                    _logger.LogWarning("Failed to reserve funds for order {OrderId}", orderId);
                    throw new PaymentFailedException("Failed to reserve funds for the order");
                }

                _logger.LogInformation("Funds reserved successfully for order {OrderId}", orderId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to reserve funds for order {OrderId}", orderId);
                throw new PaymentFailedException($"Failed to reserve funds: {ex.Message}");
            }

            // Create and save the order
            var order = new Order
            {
                Id = orderId,
                BuyerId = createOrderDto.BuyerId,
                Items = orderItems,
                TotalAmount = totalAmount,
                Status = OrderStatus.Reserved,
                CreatedAt = DateTime.UtcNow
            };

            await _orderRepository.CreateAsync(order);
            _logger.LogInformation("Order created successfully with ID: {OrderId}", order.Id);

            return MapToOrderDto(order);
        }

        public async Task<OrderDto> CompleteOrderAsync(string orderId)
        {
            _logger.LogInformation("Completing order with ID: {OrderId}", orderId);

            var order = await _orderRepository.GetByIdAsync(orderId);

            if (order == null)
            {
                _logger.LogWarning("Order with ID {OrderId} not found", orderId);
                throw new DomainException($"Order with ID {orderId} not found");
            }

            if (order.Status != OrderStatus.Reserved)
            {
                _logger.LogWarning("Order with ID {OrderId} is not in Reserved status. Current status: {Status}",
                    orderId, order.Status);
                throw new DomainException($"Order with ID {orderId} is not in Reserved status");
            }

            try
            {
                _logger.LogInformation("Completing payment for order ID: {OrderId}", orderId);

                var success = await _balanceService.CompleteOrderAsync(orderId);

                if (!success)
                {
                    _logger.LogWarning("Failed to complete payment for order {OrderId}", orderId);

                    order.Status = OrderStatus.Failed;
                    await _orderRepository.UpdateAsync(order);

                    throw new PaymentFailedException("Failed to complete payment for the order");
                }

                order.Status = OrderStatus.Completed;
                order.CompletedAt = DateTime.UtcNow;

                await _orderRepository.UpdateAsync(order);
                _logger.LogInformation("Order completed successfully with ID: {OrderId}", orderId);

                return MapToOrderDto(order);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to complete payment for order with ID: {OrderId}", orderId);

                order.Status = OrderStatus.Failed;
                await _orderRepository.UpdateAsync(order);

                throw new PaymentFailedException($"Failed to complete payment: {ex.Message}");
            }
        }

        public async Task<OrderDto?> GetOrderByIdAsync(string id)
        {
            _logger.LogInformation("Getting order with ID: {OrderId}", id);
            var order = await _orderRepository.GetByIdAsync(id);
            return order != null ? MapToOrderDto(order) : null;
        }

        public async Task<IEnumerable<OrderDto>> GetOrdersByBuyerIdAsync(string buyerId)
        {
            _logger.LogInformation("Getting orders for buyer: {BuyerId}", buyerId);
            var orders = await _orderRepository.GetOrdersByBuyerIdAsync(buyerId);
            return orders.Select(MapToOrderDto);
        }

        private OrderDto MapToOrderDto(Order order)
        {
            return new OrderDto
            {
                Id = order.Id,
                BuyerId = order.BuyerId,
                Items = order.Items.Select(i => new OrderItemDto
                {
                    ProductId = i.ProductId,
                    ProductName = i.ProductName,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice
                }).ToList(),
                TotalAmount = order.TotalAmount,
                Status = order.Status.ToString(),
                CreatedAt = order.CreatedAt,
                CompletedAt = order.CompletedAt
            };
        }
    }
}