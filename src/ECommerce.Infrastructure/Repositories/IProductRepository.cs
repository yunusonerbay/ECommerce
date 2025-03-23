using ECommerce.Domain.Entities;

namespace ECommerce.Infrastructure.Repositories;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllAsync();
    Task<Product?> GetByIdAsync(string id);
    Task UpdateStockAsync(string id, int quantity);
}