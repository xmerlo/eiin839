using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebProxyService
{
    class JCDecauxStationDetails
    {

        public JCDecauxStationDetails(string param)
        {
            string[] cacheItemInfos = param.Split('_');
            string contract = cacheItemInfos[0];
            string number = cacheItemInfos[1];
            string api_key = "74f096b23d9788282605c443fb687ea460c9e170";
            HttpClient client = new HttpClient();

            try
            {
                Task<string> response = client.GetStringAsync("https://api.jcdecaux.com/vls/v3/stations/" + number + "?contract=" + contract + "&apiKey=" + api_key);
                response.Wait();
                JCDecauxStationDetails station = JsonConvert.DeserializeObject<JCDecauxStationDetails>(response.Result);

                this.address = station.address;
                this.contractName = station.contractName;
                this.name = station.name;
                this.number = station.number;
                this.position = station.position;
                this.totalStands = station.totalStands;

                Console.WriteLine(this);
            }
            catch
            {
                Console.WriteLine("Error in calling all stations");
            }
        }
        public JCDecauxStationDetails()
        {
        }
        public int number { get; set; }
        public string contractName { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public Position position { get; set; }
        public TotalStands totalStands { get; set; }

        public JCDecauxStation toJCDecauxStation()
        {
            JCDecauxStation s = new JCDecauxStation();
            s.number = number;
            s.contractName = contractName;
            s.name = name;
            s.address = address;
            s.position = position;
            return s;
        }
        public override string ToString()
        {
            return contractName+"_"+number;
        }
    }

    class Position
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
    }

    class TotalStands
    {
        public Availabilities availabilities { get; set; }
    }

    class Availabilities
    {
        public int bikes { get; set; }
    }
}
