using System;

namespace Exo5_external_call
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 1)
                Console.WriteLine("Hello " + args[0] + " et " + args[1] + " ");
            else
                Console.WriteLine("Not arguments enough");
        }
    }
}
