using ECommerce.Domain.Entities;

namespace ECommerce.Infrastructure.Models;

public class ProductsResponse : BaseResponse
{
    public List<Product> Data { get; set; } = new List<Product>();
}