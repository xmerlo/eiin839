using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MyFirstRest
{
    class Program
    {

        static readonly HttpClient client = new HttpClient();
        private static Task<string> res;
        static async Task Main(string[] args)
        {
            try
            {

                res = getAllContracts();
               
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }


            printContracts(res.Result.ToString());
            Console.ReadKey();

        }

        private static async Task<string> getAllContracts()
        {
            HttpResponseMessage response = await client.GetAsync("https://api.jcdecaux.com/vls/v1/stations?&apiKey=74f096b23d9788282605c443fb687ea460c9e170");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        private static void printContracts(string jsonObject)
        {
            dynamic jsonRes = JsonConvert.DeserializeObject(jsonObject);            

            try
            {
                foreach (var item in jsonRes)
                {
                    Console.WriteLine(item.name);
                }
            }
            catch
            {
                Console.WriteLine("Catch");
            }
        }
    }
}
