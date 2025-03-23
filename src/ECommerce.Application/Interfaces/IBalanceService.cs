using System.Threading.Tasks;
using ECommerce.Application.DTOs;

namespace ECommerce.Application.Interfaces
{
    public interface IBalanceService
    {
        Task<BalanceDto> GetBalanceAsync();
    }
}