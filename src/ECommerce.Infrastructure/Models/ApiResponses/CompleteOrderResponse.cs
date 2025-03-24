using System;
using System.Text.Json.Serialization;
using ECommerce.Domain.Entities;

namespace ECommerce.Infrastructure.Models.ApiResponses
{
    public class CompleteOrderResponse : BaseResponse<CompleteOrderResponseData>
    {
    }

    public class CompleteOrderResponseData
    {
        [JsonPropertyName("order")]
        public OrderData? Order { get; set; }

        [JsonPropertyName("updatedBalance")]
        public Balance? UpdatedBalance { get; set; }
    }

    public class OrderData
    {
        [JsonPropertyName("orderId")]
        public string OrderId { get; set; } = string.Empty;

        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }

        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        [JsonPropertyName("completedAt")]
        public DateTime CompletedAt { get; set; }
    }
}