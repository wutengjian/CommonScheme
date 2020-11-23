using System;

namespace CommonScheme.ConfigCore
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(123);
            while (true)
            {
                if (Console.ReadLine() == "exit") break;
            }
        }
    }
}
