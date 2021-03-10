using System;

namespace calc
{
    class Program
    {
        static void Main(string[] args)
        {
            Calculator.CalculatorSoap c = new Calculator.CalculatorSoapClient();
            Console.WriteLine(c.Add(11, 11));
            Console.ReadLine();
        }
    }
}
