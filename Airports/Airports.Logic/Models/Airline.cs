using Airports.Logic.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace Airports.Logic.Models
{
    public class Airline
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
    }
}
