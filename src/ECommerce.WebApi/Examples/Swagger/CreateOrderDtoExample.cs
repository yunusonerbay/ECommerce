using System;
using System.Collections.Generic;
using ECommerce.Application.DTOs;
using Swashbuckle.AspNetCore.Filters;

namespace ECommerce.WebApi.Examples.Swagger
{
    public class CreateOrderDtoExample : IExamplesProvider<CreateOrderDto>
    {
        public CreateOrderDto GetExamples()
        {
            return new CreateOrderDto
            {
                BuyerId = "buyer-123",
                Items = new List<OrderItemRequestDto>
                {
                    new OrderItemRequestDto { ProductId = "prod-001", Quantity = 2 },
                    new OrderItemRequestDto { ProductId = "prod-002", Quantity = 1 }
                }
            };
        }
    }

    public class OrderDtoExample : IExamplesProvider<OrderDto>
    {
        public OrderDto GetExamples()
        {
            return new OrderDto
            {
                Id = "order-123",
                BuyerId = "buyer-123",
                TotalAmount = 129.99m,
                Status = "Completed",
                CreatedAt = DateTime.UtcNow,
                CompletedAt = DateTime.UtcNow.AddMinutes(5),
                Items = new List<OrderItemDto>
                {
                    new OrderItemDto
                    {
                        ProductId = "prod-001",
                        ProductName = "Smartphone",
                        Quantity = 2,
                        UnitPrice = 49.99m
                    },
                    new OrderItemDto
                    {
                        ProductId = "prod-002",
                        ProductName = "Headphones",
                        Quantity = 1,
                        UnitPrice = 30.01m
                    }
                }
            };
        }
    }

    public class ProductDtoExample : IExamplesProvider<ProductDto>
    {
        public ProductDto GetExamples()
        {
            return new ProductDto
            {
                Id = "prod-001",
                Name = "Smartphone",
                Description = "Latest model smartphone with high-end features",
                Price = 49.99m,
                Currency = "USD",
                Category = "Electronics",
                Stock = 100
            };
        }
    }

    public class ProductDtoListExample : IExamplesProvider<List<ProductDto>>
    {
        public List<ProductDto> GetExamples()
        {
            return new List<ProductDto>
            {
                new ProductDto
                {
                    Id = "prod-001",
                    Name = "Smartphone",
                    Description = "Latest model smartphone with high-end features",
                    Price = 49.99m,
                    Currency = "USD",
                    Category = "Electronics",
                    Stock = 100
                },
                new ProductDto
                {
                    Id = "prod-002",
                    Name = "Headphones",
                    Description = "Wireless noise-cancelling headphones",
                    Price = 30.01m,
                    Currency = "USD",
                    Category = "Electronics",
                    Stock = 50
                }
            };
        }
    }

    public class BalanceDtoExample : IExamplesProvider<BalanceDto>
    {
        public BalanceDto GetExamples()
        {
            return new BalanceDto
            {
                UserId = "user-123",
                TotalBalance = 10000.00m,
                AvailableBalance = 9500.00m,
                BlockedBalance = 500.00m,
                Currency = "USD",
                LastUpdated = DateTime.UtcNow
            };
        }
    }
}