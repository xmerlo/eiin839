namespace RoutingWithBikes
{
    public class JCDecauxStationDetails
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

        public override bool Equals(object obj)
        {

            var item = obj as JCDecauxStationDetails;

            if (item == null)
            {
                return false;
            }
            return item.number.Equals(number);
        }

        public override int GetHashCode()
        {
            return this.number;
        }
    }

    public class Position
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
    }

    public class TotalStands
    {
        public Availabilities availabilities { get; set; }
    }

    public class Availabilities
    {
        public int bikes { get; set; }
    }
}
