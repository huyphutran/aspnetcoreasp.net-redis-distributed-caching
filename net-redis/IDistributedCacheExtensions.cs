using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace net_redis
{
    public static class IDistributedCacheExtensions
    {
        public static async Task<GetCachedValue<T>> GetorAddAsync<T, TKey>(
            this IDistributedCache cache, 
            TKey anyKey, 
            Func<TKey, Task<T>> factory) where T : class
        {
            var key = anyKey switch
            {
                string k => k,
                _ => anyKey.ToString(),
            };

            var value = await cache.GetAsync<T>(key);
            if (value == null)
            {
                value = await factory(anyKey);
                await cache.SetStringAsync(key, JsonSerializer.Serialize(value));
                return new (false, value);
            }
            return new(true, value);
        }

        public static async Task<T> GetAsync<T>(
           this IDistributedCache cache,
           string key) where T : class
        {
            var Jsonvalue = await cache.GetStringAsync(key);
            if (string.IsNullOrEmpty(Jsonvalue))
            {
                return null;
            }
            return JsonSerializer.Deserialize<T>(Jsonvalue);

        }


        public static Task SetAsync<T>
            (this IDistributedCache cache, T value) where T : ICacheKey
        {
            return cache.SetStringAsync(value.CacheKey, JsonSerializer.Serialize(value));
        }

        public record GetCachedValue<T>(bool Cached, T value);
    }
}
