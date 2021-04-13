using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using WebProxyService;
using System.Net.Http;
using System.Threading.Tasks;
using System.ServiceModel.Web;
using Newtonsoft.Json.Linq;

namespace RoutingWithBikes
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom de classe "Service1" à la fois dans le code et le fichier de configuration.
    public class Service1 : IService1
    {

        HttpClient client = new HttpClient();
        string api_key = "5b3ce3597851110001cf624885e4173678244e2280675d80c46bd456";


        public System.ServiceModel.Channels.Message GetAllStationss(double x, double y, double x2, double y2)
        {
            WebOperationContext.Current.OutgoingResponse.Headers.Add("Access-Control-Allow-Origin", "*");
            try
            {
                Task<string> response = client.GetStringAsync("http://localhost:8733/Design_Time_Addresses/WebProxyService/Service1/GetAllStations");

                response.Wait();

                List<JCDecauxStation> list = JsonConvert.DeserializeObject<List<JCDecauxStation>>(response.Result);

                JCDecauxStations stations = new JCDecauxStations();

                stations.stations = list;

                //164042 pieds = 50km
                List<JCDecauxStation> stations1 = stations.FilterAndSortDistance(164042, x, y);

                JCDecauxStationDetails s1 = null;
                foreach(JCDecauxStation station in stations1)
                {
                    Task<string> responseS1 = client.GetStringAsync("http://localhost:8733/Design_Time_Addresses/WebProxyService/Service1/GetStation?contract="+station.contractName+"&number="+station.number);
                    responseS1.Wait();
                    JCDecauxStationDetails s = JsonConvert.DeserializeObject<JCDecauxStationDetails>(responseS1.Result);
                    if (s.totalStands.availabilities.bikes > 0)
                    {
                        s1 = s;
                        break;
                    }
                        
                }

                //164042 pieds = 50km
                List<JCDecauxStation> stations2 = stations.FilterAndSortDistance(164042, x, y);

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
                    Answer answer = new Answer();

                    answer.station1 = s1;
                    answer.station2 = s2;


                    //Start to station 1
                    string urlStartToS1 = "https://api.openrouteservice.org/v2/directions/foot-walking?api_key=" + api_key + "&start=" + y.ToString("0.00000000000000000", System.Globalization.CultureInfo.InvariantCulture) + "," + x.ToString("0.00000000000000000", System.Globalization.CultureInfo.InvariantCulture) + "&end=" + s1.position.longitude.ToString("0.00000000000000000", System.Globalization.CultureInfo.InvariantCulture) + "," + s1.position.latitude.ToString("0.00000000000000000", System.Globalization.CultureInfo.InvariantCulture);
                    Task<string> responseStartToS1 = client.GetStringAsync(urlStartToS1);
                    responseStartToS1.Wait();
                    answer.startToS1 = JObject.Parse(responseStartToS1.Result);

                    //Station 1 to station 2
                    string urlS1ToS2 = "https://api.openrouteservice.org/v2/directions/cycling-road?api_key=" + api_key + "&start=" + s1.position.longitude.ToString("0.00000000000000000", System.Globalization.CultureInfo.InvariantCulture) + "," + s1.position.latitude.ToString("0.00000000000000000", System.Globalization.CultureInfo.InvariantCulture) + "&end=" + s2.position.longitude.ToString("0.00000000000000000", System.Globalization.CultureInfo.InvariantCulture) + "," + s2.position.latitude.ToString("0.00000000000000000", System.Globalization.CultureInfo.InvariantCulture);
                    Task<string> responseS1ToS2 = client.GetStringAsync(urlS1ToS2);
                    responseS1ToS2.Wait();
                    answer.s1ToS2 = JObject.Parse(responseS1ToS2.Result);

                    //Station 2 to goal
                    string urlS2ToGoal = "https://api.openrouteservice.org/v2/directions/foot-walking?api_key=" + api_key + "&start=" + s2.position.longitude.ToString("0.00000000000000000", System.Globalization.CultureInfo.InvariantCulture) + "," + s2.position.latitude.ToString("0.00000000000000000", System.Globalization.CultureInfo.InvariantCulture) + "&end=" + y2.ToString("0.00000000000000000", System.Globalization.CultureInfo.InvariantCulture) + "," + x2.ToString("0.00000000000000000", System.Globalization.CultureInfo.InvariantCulture);
                    Task<string> responseS2ToGoal = client.GetStringAsync(urlS2ToGoal);
                    responseS2ToGoal.Wait();
                    answer.s2ToGoal = JObject.Parse(responseS2ToGoal.Result);

                    string answer_str = JsonConvert.SerializeObject(answer);

                    return WebOperationContext.Current.CreateTextResponse(answer_str,
                            "application/json; charset=utf-8",
                            Encoding.UTF8);
                    //return responseStartToS1.Result;

                }

                

                /*string result = JsonConvert.SerializeObject(s1);

                return result;*/
            }
            catch
            {
                Console.WriteLine("Error in calling all stations");
            }

            return WebOperationContext.Current.CreateTextResponse("It is better to walk",
                            "application/json; charset=utf-8",
                            Encoding.UTF8);
        }

        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }
    }
}
