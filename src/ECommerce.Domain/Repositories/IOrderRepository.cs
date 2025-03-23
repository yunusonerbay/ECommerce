using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Repositories;

public interface IOrderRepository
{

    Task<Order> CreateAsync(Order order);
    Task<Order?> GetByIdAsync(string id);
    Task UpdateAsync(Order order);
    Task<IEnumerable<Order>> GetOrdersByBuyerIdAsync(string buyerId);
}