using System;
using Microsoft.Extensions.Caching.Memory;
using MusicAppApi.Core.interfaces.Services;
using MusicAppApi.Core.Interfaces;

namespace MusicAppApi.Core.Services
{
    public class CachingService : ICachingService
    {
        private readonly IMemoryCache _cache;

        public CachingService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public T? GetFromCache<T>(string cacheKey)
        {
            return _cache.Get<T>(cacheKey);
        }

        public bool SetCache<T>(string cacheKey, T dataToCache, MemoryCacheEntryOptions cacheEntryOptions)
        {
            var res = _cache.Set(cacheKey, dataToCache, cacheEntryOptions);
            
            return res != null;
        }
    }
}