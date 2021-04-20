using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Runtime.Caching;
using System.Text.Json;
using Newtonsoft.Json;

namespace WebProxyService
{
    class MyCache<T>
        where T : class, new()
    {
        public ObjectCache cache;
        public DateTimeOffset dt_default;
        private string api_key;
        HttpClient client;

        public MyCache()
        {
            cache = MemoryCache.Default;
            dt_default = ObjectCache.InfiniteAbsoluteExpiration;
            api_key = "74f096b23d9788282605c443fb687ea460c9e170";
            client = new HttpClient();
        }

        public T Get(String CacheItem, T obj, DateTimeOffset dt)
        {
            if (cache.Get(CacheItem) == null)
            {
                Set(CacheItem, obj, dt);
            }
            return (T)cache.Get(CacheItem);
        }

        public T Get(string CacheItem, T obj, double dt_seconds)
        {
            if (cache.Get(CacheItem) == null)
            {
                Set(CacheItem, obj, dt_seconds);
            }
            return (T)cache.Get(CacheItem);
        }

        public T Get(string CacheItem, double dt_seconds)
        {
            if (cache.Get(CacheItem) == null)
            {
                Set(CacheItem, dt_seconds);
            }
            return (T)cache.Get(CacheItem);
        }

        public T Get(string CacheItem)
        {
            if (cache.Get(CacheItem) == null)
            {
                Set(CacheItem);
            }
            return (T)cache.Get(CacheItem);
        }

        public void Set(string CacheItem, T obj, DateTimeOffset dt)
        {
            var cacheItemPolicy = new CacheItemPolicy
            {
                AbsoluteExpiration = dt,
            };
            cache.Set(CacheItem, obj, cacheItemPolicy);
        }

        public void Set(string CacheItem, T obj, double dt_seconds)
        {
            var cacheItemPolicy = new CacheItemPolicy
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(dt_seconds),
            };
            cache.Set(CacheItem, obj, cacheItemPolicy);
        }

        private void Set(string CacheItem, T obj)
        {
            var cacheItemPolicy = new CacheItemPolicy
            {
                AbsoluteExpiration = dt_default,
            };

            cache.Set(CacheItem, obj, cacheItemPolicy);
        }

        private void Set(string CacheItem)
        {
            var cacheItemPolicy = new CacheItemPolicy
            {
                AbsoluteExpiration = dt_default,
            };

            T obj = (T)Activator.CreateInstance(typeof(T), CacheItem);

            cache.Set(CacheItem, obj, cacheItemPolicy);
        }

        private void Set(string CacheItem, double dt_seconds)
        {
            var cacheItemPolicy = new CacheItemPolicy
            {
                AbsoluteExpiration = dt_default,
            };

            T obj = (T)Activator.CreateInstance(typeof(T), CacheItem);

            cache.Set(CacheItem, obj, cacheItemPolicy);
        }


    }
}
