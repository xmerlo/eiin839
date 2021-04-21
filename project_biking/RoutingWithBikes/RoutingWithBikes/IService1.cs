using System.ServiceModel;
using System.ServiceModel.Web;
using System.IO;

namespace RoutingWithBikes
{
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        Stream GetRestStationsAndItinary(double lat, double lon, double lat2, double lon2, string startAdress, string goalAdress, int distanceLimit);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        string GetSoapStationsAndItinary(double lat, double lon, double lat2, double lon2, string startAdress, string goalAdress, int distanceLimit);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        string GetStatistics();

    }
}
