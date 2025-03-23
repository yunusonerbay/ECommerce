using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using ECommerce.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ECommerce.WebApi.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            
            object response = new
            {
                error = new
                {
                    message = "An error occurred while processing your request."
                }
            };

            switch (exception)
            {
                case InsufficientStockException ex:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response = new
                    {
                        error = new
                        {
                            message = ex.Message,
                            code = "INSUFFICIENT_STOCK",
                            details = new[]
                            {
                                $"Product ID: {ex.ProductId}, Requested: {ex.RequestedQuantity}, Available: {ex.AvailableQuantity}"
                            }
                        }
                    };
                    break;
                
                case PaymentFailedException ex:
                    context.Response.StatusCode = (int)HttpStatusCode.BadGateway;
                    response = new
                    {
                        error = new
                        {
                            message = ex.Message,
                            code = "PAYMENT_FAILED"
                        }
                    };
                    break;
                
                case DomainException ex:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response = new
                    {
                        error = new
                        {
                            message = ex.Message
                        }
                    };
                    break;
                
                default:
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var json = JsonSerializer.Serialize(response, options);
            await context.Response.WriteAsync(json);
        }
    }
}