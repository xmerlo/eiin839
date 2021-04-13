using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebProxyService;
using System.Net.Http;
using Newtonsoft.Json;
using System.Device.Location;

namespace RoutingWithBikes
{
    class JCDecauxStations
    {
        public List<JCDecauxStation> stations { get; set; }

        public JCDecauxStations()
        {
            //CallAllStations();
        }

        /*public async Task<string> initialize(){
            HttpClient client = new HttpClient();
            try
            {
                string response = await client.GetStringAsync("http://localhost:8733/Design_Time_Addresses/WebProxyService/Service1/GetAllStations");

                List<JCDecauxStation> aa = JsonConvert.DeserializeObject<List<JCDecauxStation>>(response);

                stations = JsonConvert.DeserializeObject<List<JCDecauxStation>>(response);

            }
            catch
            {
                Console.WriteLine("Error in calling all stations");
            }

            return Tas;
        }*/

        /*private async void CallAllStations()
        {
            HttpClient client = new HttpClient();
            try
            {
                string response = await client.GetStringAsync("http://localhost:8733/Design_Time_Addresses/WebProxyService/Service1/GetAllStations");

                List<JCDecauxStation> aa = JsonConvert.DeserializeObject<List<JCDecauxStation>>(response);

                stations = JsonConvert.DeserializeObject<List<JCDecauxStation>>(response);
                
            }
            catch
            {
                Console.WriteLine("Error in calling all stations");
            }
        }*/

        public List<JCDecauxStation> FilterAndSortDistance(double distanceLimit, double x, double y)
        {
            var goalCoordinate = new GeoCoordinate(x, y);

            List<JCDecauxStation> list = new List<JCDecauxStation>();
            
            foreach(JCDecauxStation station in stations)
            {
                var stationCoordinate = new GeoCoordinate(station.position.latitude, station.position.longitude);

                var dist = goalCoordinate.GetDistanceTo(stationCoordinate);

                if (dist < distanceLimit)
                {
                    station.distanceToGoal = dist;
                    list.Add(station);
                }
                /*else
                {
                    station.SetDistanceToGoal(dist);
                }*/

            }
            //return stations.OrderByDescending(s => s.GetDistanceToGoal()).ToList();
            return list.OrderByDescending(s => s.distanceToGoal).ToList();
            //stations.OrderBy<s=>s.>
        }
    }
}
