using LazyCache;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;
using System.Net.Http.Headers;

namespace GraphAPIService
{
    public static class ServiceCollectionExtension
    {
        /// <summary>
        /// Add GraphAPI HttpClient to .net service collection
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public static IServiceCollection AddGraphAPIServce(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddLazyCache();
            services.Configure<AuthenticationConfig>(configuration.GetSection(nameof(AuthenticationConfig)));
            services.AddSingleton<MsalAuthenticationProvider>();
            services.AddScoped<GraphClient>();

            return services;
        }
    }
}
