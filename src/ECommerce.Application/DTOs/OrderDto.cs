namespace ECommerce.Application.DTOs;

public class OrderDto
{
    public string Id { get; set; } = default!;
    public List<OrderItemDto> Items { get; set; } = new List<OrderItemDto>();
    public decimal TotalAmount { get; set; }
    public string BuyerId { get; set; } = default!;
    public string Status { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
}