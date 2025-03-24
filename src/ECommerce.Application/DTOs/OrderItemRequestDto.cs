using System.ComponentModel.DataAnnotations;

namespace ECommerce.Application.DTOs;

public class OrderItemRequestDto
{
    public string ProductId { get; set; }

    public int Quantity { get; set; }
}