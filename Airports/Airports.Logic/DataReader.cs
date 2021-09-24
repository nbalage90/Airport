using Airports.Logic.Services;
using NLog;
using System;
using System.IO;
using System.Text.RegularExpressions;
using AirportList = Airports.Logic.Models.Airports;

namespace Airports.Logic
{
    public class DataReader
    {
        readonly Logger logger;

        const string RegExp = "^[0-9]{1,4},(\".*\",){3}(\"[A-Za-z]+\",){2}([-0-9]{1,4}(\\.[0-9]{0,})?,){2}";
        const string DataPath = @"../../../../Airports.Logic/Data/airports.dat";

        public DataReader()
        {
            logger = LogManager.GetCurrentClassLogger();
        }

        public AirportList LoadAirports()
        {
            AirportList airports = new AirportList();
            var path = Path.Combine(Environment.CurrentDirectory, DataPath);

            using (StreamReader sr = File.OpenText(path))
            {
                string line = "";
                while ((line = sr.ReadLine()) != null)
                {
                    if (!IsLineProcessable(line))
                    {
                        continue;
                    }
                    var data = line.AirportSplit(',');
                    airports.Add(data);
                }
            }

            return airports;
        }

        private bool IsLineProcessable(string line)
        {
            if (!Regex.Match(line, RegExp).Success)
            {
                logger.Info($"The next line doesn't match with the pattern: {line}");
                return false;
            }
            return true;
        }
    }
}
