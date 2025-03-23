using ECommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Interfaces
{
    public interface IBalanceManagementService
    {
        Task<IEnumerable<Product>> GetProductsAsync();
        Task<string> CreatePreorderAsync(string buyerId, List<OrderItem> items, decimal totalAmount);
        Task CompleteOrderAsync(string transactionId);
    }
}
