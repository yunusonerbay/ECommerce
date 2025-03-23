using System;

namespace ECommerce.Domain.Exceptions
{
    public class InsufficientStockException : DomainException
    {
        public InsufficientStockException(string productId, int requestedQuantity, int availableQuantity)
            : base($"Insufficient stock for product {productId}. Requested: {requestedQuantity}, Available: {availableQuantity}")
        {
            ProductId = productId;
            RequestedQuantity = requestedQuantity;
            AvailableQuantity = availableQuantity;
        }

        public string ProductId { get; }
        public int RequestedQuantity { get; }
        public int AvailableQuantity { get; }
    }
}