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
    class StationCache<T>
        where T : class, new()
    {
        public ObjectCache cache;
        public DateTimeOffset dt_default;
        private string api_key;
        HttpClient client;

        public StationCache()
        {
            cache = MemoryCache.Default;
            dt_default = ObjectCache.InfiniteAbsoluteExpiration;
            api_key = "74f096b23d9788282605c443fb687ea460c9e170";
            client = new HttpClient();

            //CallAllStations();

            /* string data = " [ {\"name\": \"John Doe\", \"occupation\": \"gardener\"}, " +
     "{\"name\": \"Peter Novak\", \"occupation\": \"driver\"} ]";

             using (
                 JsonDocument doc = JsonDocument.Parse(data))
             {
                 DateTimeOffset cc = DateTimeOffset.Now.AddSeconds(1000);

                 cache.Set("aa_aa", doc, cc);

                 cache.Set("bb_bb", doc, cc);
             }*/

            //JsonDocument doc = JsonDocument.Parse(data);


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

        public async Task<T> GetAsync(string CacheItem)
        {
            if (cache.Get(CacheItem) == null)
            {
                await SearchStation(CacheItem);
                //Set(CacheItem, station);
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

        /*public async T CallApiAsync(string CacheItem)
        {
            //TODO : Call all stations maybe
            string[] coordonnees = CacheItem.Split('_');

            HttpClient client = new HttpClient();

            string request = "https://api.jcdecaux.com/vls/v1/stations?contract=marseille&apiKey=" + api_key;
            

            var response = await client.GetStreamAsync(request);
            return await JsonDocument.ParseAsync(response);
        }*/

        private async void CallAllStations()
        {

            try
            {
                string response = await client.GetStringAsync("https://api.jcdecaux.com/vls/v3/stations?apiKey=" + api_key);
                List<T> stations = JsonConvert.DeserializeObject<List<T>>(response);
                foreach (T station in stations)
                {
                    string str = station.ToString();
                    Set(station.ToString(), station, 120);
                }
            }
            catch
            {
                Console.WriteLine("Error in calling all stations");
            }

        }

        private async 
        Task
SearchStation(string CacheItem)
        {
            string[] cacheItemInfos = CacheItem.Split('_');
            string contract = cacheItemInfos[0];
            string number = cacheItemInfos[1];

            try
            {
                string response = await client.GetStringAsync("https://api.jcdecaux.com/vls/v3/stations/" + number + "?contract=" + contract + "&apiKey=" + api_key);
                T station = JsonConvert.DeserializeObject<T>(response);
                Set(station.ToString(), station, 120);
            }
            catch
            {
                Console.WriteLine("Error in calling all stations");
            }

        }

        public List<T> GetAllStations()
        {
            List<T> list = new List<T>();

            foreach (var item in cache)
            {
                list.Add((T)item.Value);
            }

            return list;
        }

    }
}
