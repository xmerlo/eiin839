namespace RoutingWithBikes
{
    class JCDecauxStation
    {
        public int number { get; set; }
        public string contractName { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public Position position { get; set; }
        public double distanceToGoal { get; set; }
        public override string ToString()
        {
            return contractName + "_" + number;
        }
    }
}
