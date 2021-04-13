using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebProxyService
{
    class JCDecauxStationDetails
    {

        public int number { get; set; }
        public string contractName { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public Position position { get; set; }
        public TotalStands totalStands { get; set; }
        public override string ToString()
        {
            return contractName + "_" + number;
        }
    }

    class Position
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
    }

    class TotalStands
    {
        public Availabilities availabilities { get; set; }
    }

    class Availabilities
    {
        public int bikes { get; set; }
    }
}
