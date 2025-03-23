using System.Threading.Tasks;
using ECommerce.Application.DTOs;
using ECommerce.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ECommerce.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BalanceController : ControllerBase
    {
        private readonly IBalanceService _balanceService;
        private readonly ILogger<BalanceController> _logger;

        public BalanceController(IBalanceService balanceService, ILogger<BalanceController> logger)
        {
            _balanceService = balanceService;
            _logger = logger;
        }

        /// <summary>
        /// Get user balance
        /// </summary>
        /// <returns>User balance information</returns>
        [HttpGet]
        [ProducesResponseType(typeof(BalanceDto), 200)]
        public async Task<ActionResult<BalanceDto>> GetBalance()
        {
            _logger.LogInformation("Getting balance information");
            var balance = await _balanceService.GetBalanceAsync();
            return Ok(balance);
        }
    }
}