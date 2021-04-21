﻿using System;
using System.ServiceModel;
using System.Threading;

namespace WebProxyServiceHost
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create the ServiceHost.
            using (ServiceHost host = new ServiceHost(typeof(WebProxyService.Service1)))
            {

                host.Open();

                Console.WriteLine("The service is ready at {0}", host.BaseAddresses[0]);
                while (true)
                {

                    // Just hang around until the container destroys the service
                    Thread.Sleep(1000);

                }

                // Close the ServiceHost - not really needed because Docker will destroy the host and us with it
                host.Close();
            }
        }
    }
}
