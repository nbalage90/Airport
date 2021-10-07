using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Airports.Logic.Models
{
    public class Airport : IEquatable<Airport>
    {
        [Column("airportId")]
        public int Id { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public int CityId { get; set; }
        public int CountryId { get; set; }
        public string IATACode { get; set; }
        public string ICAOCode { get; set; }
        public string TimeZoneName { get; set; }
        public City City { get; set; }
        public Country Country { get; set; }
        public Location Location { get; set; }

        public bool Equals(Airport other)
        {
            return other.Id == Id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }
    }
}
