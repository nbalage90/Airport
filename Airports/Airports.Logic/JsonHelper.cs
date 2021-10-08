using Airports.Logic.Entities;
using Airports.Logic.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airports.Logic
{
    public static class JsonHelper
    {
        public static void ReadTimeZones(IEnumerable<Airport> airports)
        {
            var path = Path.Combine(Environment.CurrentDirectory, AirportsConstants.DataFolder + "timezoneinfo.json");
            IEnumerable<JsonItem> items;
            using (StreamReader sr = new StreamReader(path))
            {
                string json = sr.ReadToEnd();
                items = JsonConvert.DeserializeObject<List<JsonItem>>(json);
            }

            foreach (var item in items)
            {
                airports.SingleOrDefault(a => a.Id == item.AirportId).TimeZoneName = item.TimeZoneInfoId;
            }
        }

        class JsonItem
        {
            public int AirportId { get; set; }
            public string TimeZoneInfoId { get; set; }
        }
    }
}
