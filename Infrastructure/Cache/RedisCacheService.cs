using StackExchange.Redis;
using Microsoft.Extensions.Configuration;
using IDatabase = StackExchange.Redis.IDatabase;

namespace Infrastructure.Cache
{
    namespace UserIPTracker.Infrastructure.Cache
    {
        public class RedisCacheService
        {
            private readonly IDatabase _cache;

            public RedisCacheService(IConfiguration configuration)
            {
                var redis = ConnectionMultiplexer.Connect(configuration["Redis:Host"]);
                _cache = redis.GetDatabase();
            }

            public async Task SetCacheAsync(string key, string value, TimeSpan expiration)
            {
                await _cache.StringSetAsync(key, value, expiration);
            }

            public async Task<string?> GetCacheAsync(string key)
            {
                return await _cache.StringGetAsync(key);
            }

            public async Task RemoveCacheAsync(string key)
            {
                await _cache.KeyDeleteAsync(key);
            }
        }
    }
}
