using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airports.Console.Models
{
    public class Airports : IEnumerable
    {
        List<Airport> airports;
        List<City> cities;
        List<Country> countries;
        List<Location> locations;

        public Airports()
        {
            airports = new List<Airport>();
            cities = new List<City>();
            countries = new List<Country>();
            locations = new List<Location>();
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
            var city = GetOrAddCity(cityName, countryName);
            var airport = airports.SingleOrDefault(a => a.Name == name);
            if (airport == null)
            {
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
                    Location = new Location
                    {
                        Longitude = longitude,
                        Latitude = latitude,
                        Altitude = altitude
                    }
                    // TODO: TimeZoneName
                };

                airports.Add(airport);
            }
        }

        City GetOrAddCity(string cityName, string countryName)
        {
            var country = GetOrAddCountry(countryName);
            var city = cities.SingleOrDefault(c => c.Name == cityName && c.CountryId == country.Id);
            if (city == null)
            {
                var maxId = cities.Count > 0 ? cities.Max(c => c.Id) : 0;
                city = new City
                {
                    Id = maxId + 1,
                    Name = cityName,
                    CountryId = country.Id,
                    Country = country,
                    // TODO: TimeZoneName
                };

                cities.Add(city);
            }

            return city;
        }

        Country GetOrAddCountry(string countryName)
        {
            var country = countries.SingleOrDefault(c => c.Name == countryName);
            if (country == null)
            {
                var maxId = countries.Count > 0 ? countries.Max(c => c.Id) : 0;
                country = new Country
                {
                    Id = maxId + 1,
                    Name = countryName
                    // TODO: 2, 3 letter ISO code
                };

                countries.Add(country);
            }

            return country;
        }

        public IEnumerator GetEnumerator()
        {
            return new AirportsEnumerator(airports);
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
