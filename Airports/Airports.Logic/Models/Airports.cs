using Airports.Logic.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airports.Logic.Models
{
    public class Airports : IEnumerable, IEnumerable<Airport>
    {
        List<Airport> airports;
        Cities cities;
        Countries countries;
        Locations locations;

        public Airports()
        {
            airports = new List<Airport>();
            locations = new Locations();
            countries = new Countries();
            cities = new Cities(countries);
        }

        public void Add(string[] data)
        {
            Add(int.Parse(@data[0]),
                data[1],
                data[2],
                data[3],
                data[4],
                data[5],
                double.Parse(@data[6]),
                double.Parse(@data[7]),
                double.Parse(@data[8]));
        }

        public void Add(int id, string name, string cityName, string countryName, string iata, string icao, double longitude, double latitude, double altitude)
        {
            var city = cities.GetOrAdd(cityName, countryName);
            var airport = airports.SingleOrDefault(a => a.Name == name);
            if (airport == null)
            {
                var location = locations.GetOrAdd(longitude, latitude, altitude);

                airport = new Airport
                {
                    Id = id,
                    CityId = city.Id,
                    CountryId = city.CountryId,
                    FullName = $"{name} Airport",
                    Name = name,
                    IATACode = iata,
                    ICAOCode = icao,
                    City = city,
                    Country = city.Country,
                    Location = location
                    // TODO: TimeZoneName
                };

                airports.Add(airport);
            }
        }

        public IEnumerator GetEnumerator()
        {
            return new AirportsEnumerator(airports);
        }

        IEnumerator<Airport> IEnumerable<Airport>.GetEnumerator()
        {
            return airports.GetEnumerator();
        }
    }

    class AirportsEnumerator : IEnumerator
    {
        Airport[] _airports;
        int position = -1;

        public object Current
        {
            get
            {
                try
                {
                    return _airports[position];
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        public AirportsEnumerator(List<Airport> airports)
        {
            _airports = airports.ToArray();
        }

        public bool MoveNext()
        {
            position++;
            return (position < _airports.Length);
        }

        public void Reset()
        {
            position = -1;
        }
    }
}
