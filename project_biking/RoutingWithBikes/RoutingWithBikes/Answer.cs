using Newtonsoft.Json.Linq;

namespace RoutingWithBikes
{
    class Answer
    {
        public string result { get; set; }
        public JCDecauxStationDetails station1 { get; set; }
        public JCDecauxStationDetails station2 { get; set; }

        public JObject startToS1 { get; set; }
        public JObject s1ToS2 { get; set; }
        public JObject s2ToGoal { get; set; }
    }
}
