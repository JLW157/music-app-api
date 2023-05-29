using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace MusicAppApi.Core.interfaces.Services
{
    public interface ICachingService
    {
        T? GetFromCache<T>(string cacheKey);
        bool SetCache<T>(string cacheKey, T dataToCache, MemoryCacheEntryOptions cacheEntryOptions);
    }
}
