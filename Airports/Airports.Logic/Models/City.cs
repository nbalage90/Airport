using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Airports.Logic.Models
{
    public class City : IEquatable<City>
    {
        public int Id { get; set; }
        public int CountryId { get; set; }
        [Column("cityName")]
        public string Name { get; set; }
        public string TimeZoneName { get; set; }
        public Country Country { get; set; }

        public bool Equals(City other)
        {
            return other.Name == Name
                && other.Country.Name == Country.Name;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Country.Name);
        }
    }
}
