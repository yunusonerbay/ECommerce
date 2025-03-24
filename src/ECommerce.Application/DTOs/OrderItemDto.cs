namespace ECommerce.Application.DTOs;

public class OrderItemDto
{
    public string ProductId { get; set; } 
    public string ProductName { get; set; } 
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}