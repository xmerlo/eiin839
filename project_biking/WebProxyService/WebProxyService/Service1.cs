using Newtonsoft.Json;
using System;
using System.ServiceModel.Web;
using System.Text;


namespace WebProxyService
{
    public class Service1 : IService1
    {
        private MyCache<JCDecauxStations> stationsCache = new MyCache<JCDecauxStations>();
        private MyCache<JCDecauxStationDetails> stationCache = new MyCache<JCDecauxStationDetails>();

        public System.ServiceModel.Channels.Message GetAllStations()
        {

            JCDecauxStations s = stationsCache.Get("stations", 604800);

            //604800 secondes = 7 jours
            string result = JsonConvert.SerializeObject(s.stations);
            return WebOperationContext.Current.CreateTextResponse(result,
                            "application/json; charset=utf-8",
                            Encoding.UTF8);
        }
        public System.ServiceModel.Channels.Message GetStation(string contract, int number)
        {
            JCDecauxStationDetails s = stationCache.Get(contract + "_" + number, 120);
            Console.WriteLine(s);
            // 2 minutes
            string station = JsonConvert.SerializeObject(s);
            return WebOperationContext.Current.CreateTextResponse(station,
                            "application/json; charset=utf-8",
                            Encoding.UTF8);
        }
    }
}
