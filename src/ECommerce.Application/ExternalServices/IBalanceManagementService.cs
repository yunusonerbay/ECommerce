using System.Collections.Generic;
using System.Threading.Tasks;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.ExternalServices
{
    public interface IBalanceManagementService
    {
        Task<IEnumerable<Product>> GetProductsAsync();
        Task<Balance> GetBalanceAsync();
        Task<bool> CreatePreorderAsync(string orderId, decimal amount);
        Task<bool> CompleteOrderAsync(string orderId);
    }
}