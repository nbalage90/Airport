using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Airports.Logic.Models
{
    public class Cities : IEnumerable
    {
        List<City> cities;
        Countries countries;

        public Cities(Countries countries)
        {
            cities = new List<City>();
            this.countries = countries;
        }

        public City GetOrAdd(string cityName, string countryName)
        {
            var country = countries.GetOrAdd(countryName);
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

        public IEnumerator GetEnumerator()
        {
            return new CityEnumerator(cities);
        }
    }

    class CityEnumerator : IEnumerator
    {
        City[] _cities;
        int position = -1;

        public object Current
        {
            get
            {
                try
                {
                    return _cities[position];
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        public CityEnumerator(List<City> cities)
        {
            _cities = cities.ToArray();
        }

        public bool MoveNext()
        {
            position++;
            return (position < _cities.Length);
        }

        public void Reset()
        {
            position = -1;
        }
    }
}
