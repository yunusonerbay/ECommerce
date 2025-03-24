using System.Text.Json.Serialization;

namespace ECommerce.Infrastructure.Models.ApiResponses
{
    public class BaseResponse
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;
    }

    public class BaseResponse<T> : BaseResponse
    {
        [JsonPropertyName("data")]
        public T? Data { get; set; }
    }
}