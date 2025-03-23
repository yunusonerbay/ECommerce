using System;
using System.Net.Http;
using ECommerce.Application.ExternalServices;
using ECommerce.Domain.Repositories;
using ECommerce.Infrastructure.Persistence;
using ECommerce.Infrastructure.Repositories;
using ECommerce.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;

namespace ECommerce.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Database
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("ECommerceDb"));

            // Repositories
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();

            // HTTP Client with Polly for resilience
            services.AddHttpClient<IBalanceManagementService, BalanceManagementService>(client =>
            {
                client.BaseAddress = new Uri(configuration["BalanceManagementApi:BaseUrl"] ?? "https://balance-management-pi44.onrender.com");
                client.Timeout = TimeSpan.FromSeconds(30);
            })
            .AddPolicyHandler(GetRetryPolicy())
            .AddPolicyHandler(GetCircuitBreakerPolicy());

            return services;
        }

        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }

        private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(5, TimeSpan.FromMinutes(1));
        }

        #region Explaination

        /*
          Bu metodlar, dış servislere yapılan HTTP isteklerinde hatalarla başa çıkmak için uygulanan dayanıklılık (resilience) desenlerini implemente ediyor. Polly kütüphanesini kullanarak, geçici hataları yönetmek ve sistemin daha sağlam çalışmasını sağlamak amacıyla eklenmiştir.
           
            GetRetryPolicy()
           
           Bu metod, geçici HTTP hatalarıyla karşılaşıldığında otomatik olarak yeniden deneme (retry) politikasını tanımlar:
           
      
           private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
           {
               return HttpPolicyExtensions
                   .HandleTransientHttpError()
                   .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                   .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
           }
           
           1. `HandleTransientHttpError()`: Bu metod, aşağıdaki geçici hataları yakalar:
              - NetworkError: Ağ bağlantı hataları
              - HttpRequestException: HTTP istek hataları
              - 5xx hata kodları (Sunucu hataları)
              - 408 (Request Timeout)
           
           2. `.OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.TooManyRequests)`: Bu, 429 (Too Many Requests) hatalarını da yakalamak için eklenir. Bu hata genellikle rate limiting (istek sınırlaması) uygulamalarında görülür.
           
           3. `.WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))`: Bu kısım retry davranışını tanımlar:
              - 3 kez yeniden deneme yapar
              - Her denemede "exponential backoff" stratejisi kullanır, yani:
                - İlk hata sonrası 2^1 = 2 saniye bekler
                - İkinci hata sonrası 2^2 = 4 saniye bekler
                - Üçüncü hata sonrası 2^3 = 8 saniye bekler
              
              Bu yaklaşım, "backoff" (geri çekilme) stratejisi olarak bilinir ve hedefe yapılan istekleri kademeli olarak yavaşlatarak sistemin kendini toparlamasına olanak tanır.
           
           ## GetCircuitBreakerPolicy()
           
           Bu metod, Circuit Breaker (Devre Kesici) modelini uygular:
           
           private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
           {
               return HttpPolicyExtensions
                   .HandleTransientHttpError()
                   .CircuitBreakerAsync(5, TimeSpan.FromMinutes(1));
           }

           
           Circuit Breaker deseni, sürekli başarısız olan isteklerin sistemi aşırı yüklememesi için tasarlanmıştır:
           
           1. `HandleTransientHttpError()`: Yine geçici HTTP hatalarını yakalar.
           
           2. `.CircuitBreakerAsync(5, TimeSpan.FromMinutes(1))`: Bu kısım devre kesici davranışını tanımlar:
              - 5 ardışık hata algılanırsa devre "açık" duruma geçer (istek göndermeyi durdurur)
              - Devre açık haldeyken gelen tüm istekler otomatik olarak başarısız olur (gerçek istekler yapılmaz)
              - 1 dakika sonra devre "yarı-açık" duruma geçer ve bir istek geçmesine izin verir
              - Eğer bu istek başarılı olursa, devre "kapalı" duruma geri döner (normal çalışma)
              - Eğer bu istek başarısız olursa, devre tekrar "açık" duruma geçer ve 1 dakika daha bekler
           
           Bu devre kesici modeli, başarısız olduğu bilinen bir servise sürekli istek göndererek kaynakların tükenmesini önler ve servise kendini toparlaması için zaman tanır.
           
           Bu iki politikanın birleşimi, uygulamanızı dış servislerdeki geçici problemlere karşı daha dayanıklı hale getirir. Geçici hatalarda yeniden deneme yaparken, sürekli hata veren servislere erişimi geçici olarak engeller. Bu, sistem güvenilirliğini artırır ve daha iyi bir kullanıcı deneyimi sağlar.
         */



        #endregion


    }
}