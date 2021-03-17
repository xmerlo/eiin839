using System;

namespace calc
{
    class Program
    {
        static void Main(string[] args)
        {
            MyMathOperations.MathOperationsClient c = new MyMathOperations.MathOperationsClient();
            Console.WriteLine(c.Add(11, 11));
            Console.WriteLine(c.Multiply(11, 11));
            Console.WriteLine(c.Substract(11, 11));
            Console.WriteLine(c.Divide(11, 11));
            Console.ReadLine();
        }
    }
}
