namespace Airports.Logic.Models
{
    public class Segment
    {
        public int Id { get; set; }
        public int AirlineId { get; set; }
        public int DepartureAirportId { get; set; }
        public Airport DepartureAirport { get; set; }
        public Airport ArrivalAirport { get; set; }
        public Airline Airline { get; set; }
    }
}
