using Airports.Console.Models;
using Airports.Console.Services;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AirportList = Airports.Console.Models.Airports;

namespace Airports.Console
{
    public class DataReader
    {
        readonly Logger logger;

        const string RegExp = "^[0-9]{1,4},(\".*\",){3}(\"[A-Za-z]+\",){2}([-0-9]{1,4}(\\.[0-9]{0,})?,){2}";
        const string DataPath = @"../../../Data/airports.dat";

        public DataReader()
        {
            logger = LogManager.GetCurrentClassLogger();
        }

        public void LoadAirports()
        {
            ReadData();
        }

        AirportList ReadData()
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
