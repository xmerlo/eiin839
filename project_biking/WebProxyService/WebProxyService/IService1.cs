using System.ServiceModel;
using System.ServiceModel.Web;

namespace WebProxyService
{
    [ServiceContract]
    public interface IService1
    {

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        System.ServiceModel.Channels.Message GetAllStations();

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        System.ServiceModel.Channels.Message GetStation(string contract, int number);
    }

}
