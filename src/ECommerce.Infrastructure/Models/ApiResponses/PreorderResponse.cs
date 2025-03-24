using System;
using System.Text.Json.Serialization;
using ECommerce.Domain.Entities;

namespace ECommerce.Infrastructure.Models.ApiResponses
{
    public class PreorderResponse : BaseResponse<PreorderResponseData>
    {
    }

    public class PreorderResponseData
    {
        [JsonPropertyName("preOrder")]
        public PreOrder? PreOrder { get; set; }

        [JsonPropertyName("updatedBalance")]
        public Balance? UpdatedBalance { get; set; }
    }

    public class PreOrder
    {
        [JsonPropertyName("orderId")]
        public string OrderId { get; set; } = string.Empty;

        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }

        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;
    }
}