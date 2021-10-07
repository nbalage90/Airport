using System;

namespace Airports.Logic.Models
{
    public class Location : IEquatable<Location>
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public double Altitude { get; set; }

        public bool Equals(Location other)
        {
            return other.Longitude == Longitude
                && other.Latitude == Latitude
                && other.Altitude == Altitude;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Longitude, Latitude, Altitude);
        }
    }
}
