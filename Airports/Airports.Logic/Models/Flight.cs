using System;

namespace Airports.Logic.Models
{
    public class Flight : IEquatable<Flight>
    {
        public int Id { get; set; }
        public TimeSpan ArrivalTime { get; set; }
        public TimeSpan DepartureTime { get; set; }
        public string Number { get; set; }
        public int SegmentId { get; set; }
        public Segment Segment { get; set; }

        public bool Equals(Flight other)
        {
            return other.Id == Id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }
    }
}
