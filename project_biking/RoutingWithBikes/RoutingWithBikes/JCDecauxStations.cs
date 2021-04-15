using System.Collections.Generic;
using System.Linq;
using System.Device.Location;

namespace RoutingWithBikes
{
    class JCDecauxStations
    {
        public List<JCDecauxStation> stations { get; set; }

        public List<JCDecauxStation> FilterAndSortDistance(double distanceLimit, double lat, double lon)
        {
            var goalCoordinate = new GeoCoordinate(lat, lon);

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
            }
            return list.OrderBy(s => s.distanceToGoal).ToList();
        }
    }
}
