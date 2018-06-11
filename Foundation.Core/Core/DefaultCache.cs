using System;
using System.Runtime.Caching;

namespace Foundation.Core
{
    public class DefaultCache : ICache
    {
        private readonly MemoryCache _cache;

        private static readonly TimeSpan DefaultTimeout = TimeSpan.FromHours(1);

        public DefaultCache()
        {
            _cache = new MemoryCache("DefaultSystemCache");
        }

        public TValue GetOrAdd<TValue>(string key, Func<TValue> valueCreator)
        {
            return GetOrAdd(key, valueCreator, DefaultTimeout);
        }

        public TValue GetOrAdd<TValue>(string key, Func<TValue> valueCreator, TimeSpan timeout)
        {
            if (!_cache.Contains(key))
            {
                _cache.Add(key, valueCreator(), new CacheItemPolicy { SlidingExpiration = timeout });
            }
            return (TValue)_cache.Get(key);
        }

        public void Add<TValue>(string key, TValue value)
        {
            Add(key, value, DefaultTimeout);
        }

        public void Add<TValue>(string key, TValue value, TimeSpan timeout)
        {
            _cache.Add(key, value, new CacheItemPolicy { SlidingExpiration = timeout });
        }

        public TValue Get<TValue>(string key)
        {
            return (TValue)_cache.Get(key);
        }

        public void Remove(string key)
        {
            _cache.Remove(key);
        }
    }
}
