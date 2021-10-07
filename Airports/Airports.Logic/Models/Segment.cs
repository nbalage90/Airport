using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Airports.Logic.Models
{
    public class Segment : IEquatable<Segment>
    {
        public int Id { get; set; }
        [Column("airline")]
        public int AirlineId { get; set; }
        [Column("arrivalAirport")]
        public int ArrivalAirportId { get; set; }
        [Column("departureAirport")]
        public int DepartureAirportId { get; set; }
        public Airport DepartureAirport { get; set; }
        public Airport ArrivalAirport { get; set; }
        public Airline Airline { get; set; }

        public bool Equals(Segment other)
        {
            return other.Id == Id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }
    }
}
