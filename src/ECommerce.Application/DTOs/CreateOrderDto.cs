using System.ComponentModel.DataAnnotations;

namespace ECommerce.Application.DTOs;

public class CreateOrderDto
{
    public string BuyerId { get; set; }

    public List<OrderItemRequestDto> Items { get; set; } 
}