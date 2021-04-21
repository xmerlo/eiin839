using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace BikingHeavyClient
{
    class Program
    {
        static void Main(string[] args)
        {
            RoutingWithBikes.Service1Client s = new RoutingWithBikes.Service1Client();
            Console.WriteLine("Same position, must return that it is bette to walk :");
            Console.WriteLine("_____________________________________________________");
            Console.WriteLine(s.GetSoapStationsAndItinary(43.567221153708445, 1.467678323560479, 43.567221153708449, 1.467678323560478, null, null, 0));
            Console.WriteLine("Itinary with just coordinates :");
            Console.WriteLine("_______________________________");
            Console.WriteLine(s.GetSoapStationsAndItinary(43.644132925978056, 1.4323214205002373, 43.567221153708449, 1.467678323560478, null, null, 0));
            Console.WriteLine("Itinary with coordinates, adresses and a distance limit :");
            Console.WriteLine("_________________________________________________________");
            Console.WriteLine(s.GetSoapStationsAndItinary(0, 0, 0, 0, "12 Chemin de la Glacière, 31200 Toulouse", "1 Rue Claude-Marie Perroud, 31000 Toulouse", 10));
            Console.WriteLine("Itinary with just adresses :");
            Console.WriteLine("____________________________");
            Console.WriteLine(s.GetSoapStationsAndItinary(43.567221153708445, 1.467678323560479, 0, 0, null, "1 Rue Claude-Marie Perroud, 31000 Toulouse", 0));
            Console.WriteLine("____________________________");
            Console.WriteLine("Statistics :");
            Dictionary<string, int> statistics = JsonConvert.DeserializeObject<Dictionary<string, int>>(s.GetStatistics());
            foreach (KeyValuePair<string, int> stat in statistics)
            {
                Console.WriteLine("Station: {0}, Request number: {1}",
                    stat.Key, stat.Value);
            }
            Console.WriteLine();
            Console.ReadLine();
        }
    }
}
