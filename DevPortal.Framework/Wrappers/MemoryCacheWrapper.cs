using DevPortal.Framework.Abstract;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace DevPortal.Framework.Wrappers
{
    public class MemoryCacheWrapper : ICacheWrapper
    {
        readonly IMemoryCache memoryCache;

        public MemoryCacheWrapper(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }

        public T Get<T>(string key)
        {
            return (T)memoryCache.Get(key);
        }

        /// <summary>
        /// <seealso cref="Microsoft.Extensions.Caching.Memory.CacheExtensions.GetOrCreate{TItem}(IMemoryCache, object, Func{ICacheEntry, TItem})"/> metodu kullanılmıştır.
        /// Ayrıntılı bilgi için https://docs.microsoft.com/en-us/aspnet/core/performance/caching/memory?view=aspnetcore-3.1
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="factory"></param>
        /// <param name="cacheTimeInMinues"></param>
        /// <returns></returns>
        public T GetOrCreateWithSlidingExpiration<T>(string key, Func<T> factory, int cacheTimeInMinutes)
        {
            return memoryCache.GetOrCreate(key,
                entry =>
                {
                    entry.SlidingExpiration = TimeSpan.FromMinutes(cacheTimeInMinutes);
                    return factory();
                });
        }

        public void AddWithAbsoluteExpiration(string key, object data, int cacheTime)
        {
            if (data == null)
            {
                return;
            }

            memoryCache.Set(key, data, DateTime.Now + TimeSpan.FromMinutes(cacheTime));
        }

        public void Remove(string key)
        {
            memoryCache.Remove(key);
        }
    }
}