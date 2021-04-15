using System.Text;
using System.ServiceModel.Web;
using System.IO;

namespace RoutingWithBikes
{
    public class Service1 : IService1
    {

        public Stream GetRestStationsAndItinary(double lat, double lon, double lat2, double lon2, string startAdress, string goalAdress, int distanceLimit)
        {

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


    }
}
