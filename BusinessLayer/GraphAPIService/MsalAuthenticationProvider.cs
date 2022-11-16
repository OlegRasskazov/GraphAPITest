using System.Net.Http.Headers;
using Microsoft.Identity.Client;
using Microsoft.Graph;
using LazyCache;
using Microsoft.Extensions.Options;

namespace GraphAPIService
{
    public class MsalAuthenticationProvider : IAuthenticationProvider
    {
        private readonly IAppCache cache;
        private readonly AuthenticationConfig config;

        public MsalAuthenticationProvider(IAppCache cache, IOptions<AuthenticationConfig> config)
        {
            this.cache = cache;
            this.config = config.Value;
        }


        public async Task AuthenticateRequestAsync(HttpRequestMessage request)
        {

            AuthenticationResult? authResult = null;
            do
            {
                authResult = await cache.GetOrAddAsync("AADACCESS", _ => GetAccessTokenAsync(config));
                if (authResult.ExpiresOn <= DateTimeOffset.Now) cache.Remove("AADACCESS");
                else break;
            }
            while (true);

            request.Headers.Authorization = new AuthenticationHeaderValue("bearer", authResult.AccessToken);
        }

        private static async Task<AuthenticationResult> GetAccessTokenAsync(AuthenticationConfig config)
        {
            var app = ConfidentialClientApplicationBuilder.Create(config.ClientId)
                .WithClientSecret(config.ClientSecret)
                .WithAuthority(new Uri(config.Authority))
                .Build();
            string[] scopes = new string[] { $"{config.ApiUrl}.default" };

            var result = await app.AcquireTokenForClient(scopes)
                .ExecuteAsync();
            return result;
        }
    }
}