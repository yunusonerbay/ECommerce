using System.Threading.Tasks;
using ECommerce.Application.DTOs;
using ECommerce.Application.ExternalServices;
using ECommerce.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace ECommerce.Application.Services
{
    public class BalanceService : IBalanceService
    {
        private readonly IBalanceManagementService _balanceManagementService;
        private readonly ILogger<BalanceService> _logger;

        public BalanceService(
            IBalanceManagementService balanceManagementService,
            ILogger<BalanceService> logger)
        {
            _balanceManagementService = balanceManagementService;
            _logger = logger;
        }

        public async Task<BalanceDto> GetBalanceAsync()
        {
            _logger.LogInformation("Getting balance information");

            var balance = await _balanceManagementService.GetBalanceAsync();

            return new BalanceDto
            {
                UserId = balance.UserId,
                TotalBalance = balance.TotalBalance,
                AvailableBalance = balance.AvailableBalance,
                BlockedBalance = balance.BlockedBalance,
                Currency = balance.Currency,
                LastUpdated = balance.LastUpdated
            };
        }
    }
}