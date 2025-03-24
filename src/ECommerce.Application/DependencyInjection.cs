using ECommerce.Application.Interfaces;
using ECommerce.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Register application services
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IBalanceService, BalanceService>();
            services.AddScoped<IOrderService, OrderService>();

            return services;
        }
    }
}