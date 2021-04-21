using System.Text;
using System.ServiceModel.Web;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace RoutingWithBikes
{
    public class Service1 : IService1
    {
        public static Dictionary<JCDecauxStationDetails, int> statistics = new Dictionary<JCDecauxStationDetails, int>();

        public Stream GetRestStationsAndItinary(double lat, double lon, double lat2, double lon2, string startAdress, string goalAdress, int distanceLimit)
        {
            WebOperationContext.Current.OutgoingResponse.Headers.Add("Access-Control-Allow-Origin", "*");

            if (startAdress != null)
            {
                Position coordinates = Utils.adressToCoordinates(startAdress);
                lat = coordinates.latitude;
                lon = coordinates.longitude;
            }

            if (goalAdress != null)
            {
                Position coordinates = Utils.adressToCoordinates(goalAdress);
                lat2 = coordinates.latitude;
                lon2 = coordinates.longitude;
            }

            WebOperationContext.Current.OutgoingResponse.ContentType =
            "application/json; charset=utf-8";
            return new MemoryStream(Encoding.UTF8.GetBytes(Utils.searchStationsAndItinaries(lat, lon, lat2, lon2, distanceLimit)));
        }

        public string GetSoapStationsAndItinary(double lat, double lon, double lat2, double lon2, string startAdress, string goalAdress, int distanceLimit)
        {
            WebOperationContext.Current.OutgoingResponse.Headers.Add("Access-Control-Allow-Origin", "*");

            if (startAdress != null)
            {
                Position coordinates = Utils.adressToCoordinates(startAdress);
                lat = coordinates.latitude;
                lon = coordinates.longitude;
            }

            if (goalAdress != null)
            {
                Position coordinates = Utils.adressToCoordinates(goalAdress);
                lat2 = coordinates.latitude;
                lon2 = coordinates.longitude;
            }

            return Utils.searchStationsAndItinaries(lat, lon, lat2, lon2, distanceLimit);
        }

        public string GetStatistics()
        {
            WebOperationContext.Current.OutgoingResponse.Headers.Add("Access-Control-Allow-Origin", "*");

            return JsonConvert.SerializeObject(statistics);
        }


    }
}
