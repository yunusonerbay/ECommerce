namespace ECommerce.Application.DTOs;

public class BalanceDto
{
    public string UserId { get; set; } = default!;
    public decimal TotalBalance { get; set; }
    public decimal AvailableBalance { get; set; }
    public decimal BlockedBalance { get; set; }
    public string Currency { get; set; } = default!;
    public DateTime LastUpdated { get; set; }
}