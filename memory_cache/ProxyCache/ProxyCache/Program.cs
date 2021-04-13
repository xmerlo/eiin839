using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Caching;
using System.Threading;
namespace ProxyCache
{
    class Program
    {

        static public Dictionary<string, int> subscribers = new Dictionary<string, int>(){
            {"aa", 10},
            {"bb", 20},
            {"cc", 30}
        };

        static public ObjectCache cache = MemoryCache.Default;

        static public void ajouterSub(string user)
        {
            subscribers.Add( user, 1);

            foreach (KeyValuePair<string, int> entry in subscribers)
            {
                // do something with entry.Value or entry.Key
                Console.WriteLine(entry.Key);
            }
            
        }

       static public void Get(string CacheItem)
        {
            if (cache.Get(CacheItem) == null)
            {
                var cacheItemPolicy = new CacheItemPolicy
                {
                    AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(5.0),
                };

                cache.Add(CacheItem, 1, cacheItemPolicy);
            }
            PrintAllCache(cache);
        }

        static void Main(string[] args)
        {
            
            ajouterSub("assssa");
            Get("aaaaa");
            Get("bbbbb");
            while (true) {
                Thread.Sleep(2000);
                PrintAllCache(cache);
            }
        }

        public static void PrintAllCache(ObjectCache cache)
        {

            DateTime dt = DateTime.Now;

            Console.WriteLine("All key-values at " + dt.Second);
            //loop through all key-value pairs and print them
            foreach (var item in cache)
            {
                Console.WriteLine("cache object key-value: " + item.Key + "-" + item.Value);
            }
        }


    }
}
