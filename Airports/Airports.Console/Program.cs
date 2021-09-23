using System;

namespace Airports.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            DataReader reader = new DataReader();
            reader.LoadAirports();

            System.Console.WriteLine("Hello World!");
        }
    }
}
