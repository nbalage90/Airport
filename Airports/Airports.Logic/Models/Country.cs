using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Airports.Logic.Models
{
    public class Country : IEquatable<Country>
    {
        public int Id { get; set; }
        [Column("countryName")]
        public string Name { get; set; }
        public string ThreeLetterISOCode { get; set; }
        public string TwoLetterISOCode { get; set; }

        public bool Equals(Country other)
        {
            return other.Name == Name;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name);
        }
    }
}
