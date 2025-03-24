using ECommerce.Application.DTOs;

namespace ECommerce.Application.Interfaces;

public interface IOrderService
{
    Task<OrderDto> CreateOrderAsync(CreateOrderDto createOrderDto);
    Task<OrderDto> CompleteOrderAsync(string orderId);
    Task<OrderDto?> GetOrderByIdAsync(string id);
    Task<IEnumerable<OrderDto>> GetOrdersByBuyerIdAsync(string buyerId);
}