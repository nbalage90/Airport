using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airports.Logic.Models
{
    public class Locations : IEnumerable
    {
        List<Location> locations;

        public Locations()
        {
            locations = new List<Location>();
        }

        public Location GetOrAdd(double longitude, double latitude, double altitude)
        {
            var location = locations.SingleOrDefault(l => l.Altitude == altitude && l.Latitude == latitude && l.Longitude == longitude);
            if (location == null)
            {
                location = new Location
                {
                    Altitude = altitude,
                    Longitude = longitude,
                    Latitude = latitude
                };
                locations.Add(location);
            }
            return location;
        }

        public IEnumerator GetEnumerator()
        {
            return new LocationEnumerator(locations);
        }
    }

    class LocationEnumerator : IEnumerator
    {
        Location[] _locations;
        int position = -1;

        public object Current
        {
            get
            {
                try
                {
                    return _locations[position];
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        public LocationEnumerator(List<Location> locations)
        {
            _locations = locations.ToArray();
        }

        public bool MoveNext()
        {
            position++;
            return (position < _locations.Length);
        }

        public void Reset()
        {
            position = -1;
        }
    }
}
