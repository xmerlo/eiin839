using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Text.Json;

namespace WebProxyService
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom de classe "Service1" à la fois dans le code et le fichier de configuration.
    public class Service1 : IService1
    {
        private StationsCache<JCDecauxStationDetails> stationsCache = new StationsCache<JCDecauxStationDetails>();
        private StationCache<JCDecauxStationDetails> stationCache = new StationCache<JCDecauxStationDetails>();

        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public System.ServiceModel.Channels.Message GetAllStations()
        {
            List<JCDecauxStationDetails> listDetails = stationsCache.GetAllStationsAsync().Result;
            List<JCDecauxStation> list = new List<JCDecauxStation>();

            foreach (JCDecauxStationDetails s in listDetails)
            {
                list.Add(s.toJCDecauxStation());
            }
            string result = JsonConvert.SerializeObject(list);
            return WebOperationContext.Current.CreateTextResponse(result,
                            "application/json; charset=utf-8",
                            Encoding.UTF8);
            //zz = new MemoryStream(Encoding.UTF8.GetBytes(result)); ;
            //return new MemoryStream(Encoding.UTF8.GetBytes(result));
            //return result;
        }
        public System.ServiceModel.Channels.Message GetStation(string contract, int number)
        {
            string station = JsonConvert.SerializeObject(stationCache.GetAsync(contract + "_" + number).Result);
            return WebOperationContext.Current.CreateTextResponse(station,
                            "application/json; charset=utf-8",
                            Encoding.UTF8);
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
