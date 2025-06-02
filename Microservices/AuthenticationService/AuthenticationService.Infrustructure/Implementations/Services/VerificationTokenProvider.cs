using AuthenticationService.Applicaion.Abstractions.Services;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;

namespace AuthenticationService.Infrastructure.Implementations.Services
{
    public class VerificationTokenProvider(IMemoryCache cache) : IVerificationTokenProvider
    {
        private readonly TimeSpan _defaultExpiry = TimeSpan.FromHours(24);

        public async Task<string> GenerateTokenAsync(string userId)
        {
            var cacheKey = GetCacheKey(userId);
            var storedToken = cache.Get(cacheKey);

            if (storedToken != null)
                cache.Remove(cacheKey);

            try
            {
                var token = Guid.NewGuid().ToString("N");

                cache.Set(cacheKey, token, _defaultExpiry);
                return token;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
           
        }

        public async Task<bool> ValidateTokenAsync(string userId, string token)
        {
            var cacheKey = GetCacheKey(userId);
            var storedToken = cache.Get(cacheKey);

            return token.Equals(storedToken);
        }

        public async Task RemoveTokenAsync(string userId)
        {
            var cacheKey = GetCacheKey(userId);
            cache.Remove(cacheKey);
        }

        private static string GetCacheKey(string userId) => $"verification_token_{userId}";
    }
}
