using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebProxyService;

namespace RoutingWithBikes
{
    class Answer
    {
        public JCDecauxStationDetails station1 { get; set; }
        public JCDecauxStationDetails station2 { get; set; }

        public JObject startToS1 { get; set; }
        public JObject s1ToS2 { get; set; }
        public JObject s2ToGoal { get; set; }
    }
}
