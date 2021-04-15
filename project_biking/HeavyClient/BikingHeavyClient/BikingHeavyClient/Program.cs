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
            ServiceReference1.Service1Client s = new ServiceReference1.Service1Client();
            Console.WriteLine(s.GetSoapStationsAndItinary(43.567221153708445, 1.467678323560479, 43.567221153708449, 1.467678323560478));
            Console.ReadLine();
        }
    }
}
