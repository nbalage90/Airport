using Airports.Logic.Attributes;
using System;
using System.Collections;
using System.ComponentModel.DataAnnotations.Schema;

namespace Airports.Logic.Models
{
    public class Airline : IEquatable<Airline>
    {
        public int Id { get; set; }

        [Column("callsign")]
        public string CallSign { get; set; }

        [Column("iata")]
        [NotEmpty]
        public string IATACode { get; set; }

        [Column("icao")]
        public string ICAOCode { get; set; }
        public string Name { get; set; }

        public bool Equals(Airline other)
        {
            return other.Id == Id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }
    }
}
