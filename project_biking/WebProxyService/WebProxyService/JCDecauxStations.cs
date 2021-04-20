using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebProxyService
{
    class JCDecauxStations
    {
        public JCDecauxStations(string param)
        {
            string api_key = "74f096b23d9788282605c443fb687ea460c9e170";
            HttpClient client = new HttpClient();

            try
            {
                Task<string> response = client.GetStringAsync("https://api.jcdecaux.com/vls/v3/stations?apiKey=" + api_key);
                response.Wait();
                List<JCDecauxStation> answer = JsonConvert.DeserializeObject<List<JCDecauxStation>>(response.Result);

                this.stations = answer;
                Console.WriteLine(this);
            }
            catch
            {
                Console.WriteLine("Error in calling all stations");
            }
        }
        public JCDecauxStations()
        {
        }

        public List<JCDecauxStation> stations { get; set; }
    }
}
