using System;
using Microsoft.Extensions.Caching.Memory;

namespace ESchool.Services.Infrastructure.Cache
{
    public class MemoryCacheService : IMemoryCacheService
    {
        private const int Sliding = 2;
        private const int Absolute = 1;
        private const int Never = 0;

        private readonly IMemoryCache _memoryCache;

        public MemoryCacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public T GetNeverExpiration<T>(string key, Func<T> acquire)
        {
            return Get(key, acquire, Never, 0);
        }

        public T GetSlidingExpiration<T>(string key, Func<T> acquire, int cacheInMinutes = 120)
        {
            return Get(key, acquire, Sliding, cacheInMinutes);
        }

        public T GetAbsoluteExpiration<T>(string key, Func<T> acquire, int cacheInMinutes = 120)
        {
            return Get(key, acquire, Absolute, cacheInMinutes);
        }

        public void Remove(string key)
        {
            _memoryCache.Remove(key);
        }

        private T Get<T>(string key, Func<T> acquire, int expiration, int cacheInMinutes)
        {
            T value;

            if (_memoryCache.TryGetValue(key, out value))
            {
                return value;
            }

            var result = acquire();

            if (result != null)
            {
                _memoryCache.Set(key, result, GetMemoryCacheEntryOptions(expiration, cacheInMinutes));
            }

            return result;
        }

        private MemoryCacheEntryOptions GetMemoryCacheEntryOptions(int expiration, int cacheInMinutes)
        {
            var memoryOptions = new MemoryCacheEntryOptions();

            switch(expiration)
            {
                case Sliding:
                    memoryOptions.SetSlidingExpiration(TimeSpan.FromMinutes(cacheInMinutes));
                    break;

                case Absolute:
                    memoryOptions.SetAbsoluteExpiration(TimeSpan.FromMinutes(cacheInMinutes));
                    break;

                case Never:
                default:
                    memoryOptions.SetPriority(CacheItemPriority.NeverRemove);
                    break;
            }

            return memoryOptions;
        }
    }
}