using System.Collections.Generic;
using System.Threading.Tasks;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.ExternalServices
{
    public interface IBalanceManagementService
    {
        Task<IEnumerable<Product>> GetProductsAsync();
        Task<Balance> GetBalanceAsync();
        Task<string> CreatePreorderAsync(string buyerId, List<OrderItem> items, decimal totalAmount);
        Task CompleteOrderAsync(string transactionId);
    }
}