using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.ServiceModel.Web;
using System.Threading.Tasks;

namespace RoutingWithBikes
{
    class Utils
    {
        static string api_key = "5b3ce3597851110001cf624885e4173678244e2280675d80c46bd456";
        static HttpClient client = new HttpClient();

        public static string searchStationsAndItinaries(double lat, double lon, double lat2, double lon2, int distanceLimit)
        {
            
            

            WebOperationContext.Current.OutgoingResponse.Headers.Add("Access-Control-Allow-Origin", "*");
            Answer answer = new Answer();
            try
            {
                Task<string> response = client.GetStringAsync("http://localhost:8733/Design_Time_Addresses/WebProxyService/Service1/GetAllStations");

                response.Wait();

                List<JCDecauxStation> list = JsonConvert.DeserializeObject<List<JCDecauxStation>>(response.Result);

                JCDecauxStations stations = new JCDecauxStations();

                stations.stations = list;

                if (distanceLimit == 0)
                    //50km
                    distanceLimit = 164042;
                else
                    //Convert km to feet
                    distanceLimit = distanceLimit * 3280;

                List<JCDecauxStation> stations1 = stations.FilterAndSortDistance(distanceLimit, lat, lon);

                JCDecauxStationDetails s1 = null;
                foreach (JCDecauxStation station in stations1)
                {
                    Task<string> responseS1 = client.GetStringAsync("http://localhost:8733/Design_Time_Addresses/WebProxyService/Service1/GetStation?contract=" + station.contractName + "&number=" + station.number);
                    responseS1.Wait();
                    JCDecauxStationDetails s = JsonConvert.DeserializeObject<JCDecauxStationDetails>(responseS1.Result);
                    if (s.totalStands.availabilities.bikes > 0)
                    {
                        s1 = s;
                        break;
                    }

                }

                List<JCDecauxStation> stations2 = stations.FilterAndSortDistance(distanceLimit, lat2, lon2);

                JCDecauxStationDetails s2 = null;
                foreach (JCDecauxStation station in stations2)
                {
                    Task<string> responseS2 = client.GetStringAsync("http://localhost:8733/Design_Time_Addresses/WebProxyService/Service1/GetStation?contract=" + station.contractName + "&number=" + station.number);
                    responseS2.Wait();
                    JCDecauxStationDetails s = JsonConvert.DeserializeObject<JCDecauxStationDetails>(responseS2.Result);
                    if (s.totalStands.availabilities.bikes > 0)
                    {
                        s2 = s;
                        break;
                    }

                }

                if (s1.number != s2.number || s1.number == s2.number)
                {
                    

                    answer.station1 = s1;
                    answer.station2 = s2;


                    //Start to station 1
                    string urlStartToS1 = "https://api.openrouteservice.org/v2/directions/foot-walking?api_key=" + api_key + "&start=" + lon.ToString("0.00000000000000000", System.Globalization.CultureInfo.InvariantCulture) + "," + lat.ToString("0.00000000000000000", System.Globalization.CultureInfo.InvariantCulture) + "&end=" + s1.position.longitude.ToString("0.00000000000000000", System.Globalization.CultureInfo.InvariantCulture) + "," + s1.position.latitude.ToString("0.00000000000000000", System.Globalization.CultureInfo.InvariantCulture);
                    Task<string> responseStartToS1 = client.GetStringAsync(urlStartToS1);
                    responseStartToS1.Wait();
                    answer.startToS1 = JObject.Parse(responseStartToS1.Result);

                    //Station 1 to station 2
                    string urlS1ToS2 = "https://api.openrouteservice.org/v2/directions/cycling-road?api_key=" + api_key + "&start=" + s1.position.longitude.ToString("0.00000000000000000", System.Globalization.CultureInfo.InvariantCulture) + "," + s1.position.latitude.ToString("0.00000000000000000", System.Globalization.CultureInfo.InvariantCulture) + "&end=" + s2.position.longitude.ToString("0.00000000000000000", System.Globalization.CultureInfo.InvariantCulture) + "," + s2.position.latitude.ToString("0.00000000000000000", System.Globalization.CultureInfo.InvariantCulture);
                    Task<string> responseS1ToS2 = client.GetStringAsync(urlS1ToS2);
                    responseS1ToS2.Wait();
                    answer.s1ToS2 = JObject.Parse(responseS1ToS2.Result);

                    //Station 2 to goal
                    string urlS2ToGoal = "https://api.openrouteservice.org/v2/directions/foot-walking?api_key=" + api_key + "&start=" + s2.position.longitude.ToString("0.00000000000000000", System.Globalization.CultureInfo.InvariantCulture) + "," + s2.position.latitude.ToString("0.00000000000000000", System.Globalization.CultureInfo.InvariantCulture) + "&end=" + lon2.ToString("0.00000000000000000", System.Globalization.CultureInfo.InvariantCulture) + "," + lat2.ToString("0.00000000000000000", System.Globalization.CultureInfo.InvariantCulture);
                    Task<string> responseS2ToGoal = client.GetStringAsync(urlS2ToGoal);
                    responseS2ToGoal.Wait();
                    answer.s2ToGoal = JObject.Parse(responseS2ToGoal.Result);

                    answer.result = "Ok";

                    return JsonConvert.SerializeObject(answer);
                }
            }
            catch
            {
                Console.WriteLine("Error in calling all stations");
            }

            answer.result = "It is better to walk";
            return JsonConvert.SerializeObject(answer);
        }

        public static Position adressToCoordinates(string adress)
        {
            

            try
            {
                Task<string> response = client.GetStringAsync("https://api.openrouteservice.org/geocode/search?api_key="+api_key+"&text="+ Uri.EscapeUriString(adress));

                response.Wait();

                JObject responseObject = JsonConvert.DeserializeObject<JObject>(response.Result);

                double longitude = (double)responseObject["features"][0]["geometry"]["coordinates"][0];
                double latitude = (double)responseObject["features"][0]["geometry"]["coordinates"][1];

                Position position = new Position();

                position.longitude = longitude;
                position.latitude = latitude;

                return position;

            }
            catch
            {
                Console.WriteLine("Error in converting adress to coordinates");
            }



            return null;
        }
    }
}
