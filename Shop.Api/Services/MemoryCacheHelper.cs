using Microsoft.Extensions.Caching.Memory;
using System;

namespace Shop.Api.Services
{
    public class MemoryCacheHelper : ICacheHelper,IDisposable
    {
        private IMemoryCache _cache;

        public MemoryCacheHelper()
        {
            _cache = new MemoryCache(new MemoryCacheOptions());
        }

        public bool Exists(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));
            object v = null;
            return _cache.TryGetValue<object>(key, out v);
        }


        public T GetCache<T>(string key) where T : class
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));
            T v = null;
            _cache.TryGetValue<T>(key, out v);
            return v;
        }


        public void SetCache(string key, object value)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            object v = null;
            if (_cache.TryGetValue(key, out v))
                _cache.Remove(key);
            _cache.Set<object>(key, value);
        }


        public void SetCache(string key, object value, double expirationMinute)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            object v = null;
            if (_cache.TryGetValue(key, out v))
                _cache.Remove(key);
            DateTime now = DateTime.Now;
            TimeSpan ts = now.AddMinutes(expirationMinute) - now;
            _cache.Set<object>(key, value, ts);
        }


        public void SetCache(string key, object value, DateTimeOffset expirationTime)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            object v = null;
            if (_cache.TryGetValue(key, out v))
                _cache.Remove(key);

            _cache.Set<object>(key, value, expirationTime);
        }


        public void RemoveCache(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            _cache.Remove(key);
        }

        public void Dispose()
        {
            if (_cache != null)
                _cache.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
