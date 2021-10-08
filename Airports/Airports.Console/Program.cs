using Airports.Logic;
using Airports.Logic.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Airports.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var locations = CsvHelper.Parse<Location>("airports.dat");
            var countries = CsvHelper.Parse<Country>("airports.dat");
            var cities = CsvHelper.Parse<City>("airports.dat");
            var airports = CsvHelper.Parse<Airport>("airports.dat");
            var airlines = CsvHelper.Parse<Airline>("airlines.dat");
            var segments = CsvHelper.Parse<Segment>("segments.dat");
            var flights = CsvHelper.Parse<Flight>("flights.dat");

            JsonHelper.ReadTimeZones(airports);

            //GetCountriesAndAirportNumbers(airports);
            //GetCityWithTheMostAirports(airports);
            //CountriesAndAirports(airports);
        }

        static void WriteLines()
        {
            System.Console.WriteLine();
            System.Console.WriteLine("----------");
        }

        static void GetCountriesAndAirportNumbers(IEnumerable<Airport> airports)
        {
            WriteLines();
            System.Console.WriteLine("List all the countries by name in an ascending order, and display the number of airports they have");
            var groupedCountries = airports.OrderBy(a => a.Country.Name).GroupBy(a => a.Country);
            foreach (var country in groupedCountries)
            {
                System.Console.WriteLine($"{country.Key.Name}: {country.Count()}");
            }
        }

        static void GetCityWithTheMostAirports(IEnumerable<Airport> airports)
        {
            WriteLines();
            System.Console.WriteLine("Find the city which has got the most airports. If there are more than one cities with the same amount, display all of them.");
            var cities = airports.GroupBy(a => a.City.Name);
            var maxCount = -1;
            var citiesWithMaxCount = new List<string>();
            foreach (var city in cities)
            {
                var cityCount = city.Count();
                if (cityCount > maxCount)
                {
                    maxCount = cityCount;
                    citiesWithMaxCount.Clear();
                    citiesWithMaxCount.Add(city.Key);
                    continue;
                }
                if (cityCount == maxCount)
                {
                    citiesWithMaxCount.Add(city.Key);
                }
            }
            foreach (var city in citiesWithMaxCount)
            {
                System.Console.WriteLine($"City: {city}, count: {maxCount}");
            }
        }

        static void CountriesAndAirports(IEnumerable<Airport> airports)
        {
            WriteLines();
            System.Console.WriteLine("List all the countries by name in an descending order, and display the name of their airports.");
            var countries = airports.OrderBy(a => a.Country.Name)
                                    .GroupBy(a => a.Country, (country, airports) => new { Country = country, Airports = airports });
            foreach (var country in countries)
            {
                System.Console.WriteLine($"{country.Country.Name}");
                foreach (var airport in country.Airports)
                {
                    System.Console.WriteLine($"  {airport.FullName}");
                }
            }
        }
    }
}
