using ECommerce.Domain.Entities;

namespace ECommerce.Infrastructure.Models;

public class BalanceResponse : BaseResponse
{
    public Balance? Data { get; set; }
}