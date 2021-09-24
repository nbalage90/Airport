using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Airports.Logic.Models
{
    public class Countries : IEnumerable
    {
        List<Country> countries;

        public Countries()
        {
            countries = new List<Country>();
        }

        public Country GetOrAdd(string countryName)
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
            return new CountryEnumerator(countries);
        }
    }

    class CountryEnumerator : IEnumerator
    {
        Country[] _countries;
        int position = -1;

        public object Current
        {
            get
            {
                try
                {
                    return _countries[position];
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        public CountryEnumerator(List<Country> countries)
        {
            _countries = countries.ToArray();
        }

        public bool MoveNext()
        {
            position++;
            return (position < _countries.Length);
        }

        public void Reset()
        {
            position = -1;
        }
    }
}
