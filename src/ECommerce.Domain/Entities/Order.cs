using ECommerce.Domain.Enums;

namespace ECommerce.Domain.Entities;

public class Order
{
    public string Id { get; set; } 
    public List<OrderItem> Items { get; set; } = new List<OrderItem>();
    public decimal TotalAmount { get; set; }
    public string BuyerId { get; set; } 
    public OrderStatus Status { get; set; }
    public string? TransactionId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
}