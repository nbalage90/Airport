﻿namespace Airports.Logic.Models
{
    public class City
    {
        public int Id { get; set; }
        public int CountryId { get; set; }
        public string Name { get; set; }
        public string TimeZoneName { get; set; }
        public Country Country { get; set; }
    }
}
