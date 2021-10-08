using Airports.Logic.Entities;
using Airports.Logic.Models;
using Airports.Logic.Services;
using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Airports.Logic
{
    public static class CsvHelper
    {
        static Dictionary<Type, IEnumerable<object>> _dict;
        public static Dictionary<Type, IEnumerable<object>> Dictionary
        {
            get
            {
                if (_dict == null)
                {
                    _dict = new Dictionary<Type, IEnumerable<object>>();
                    return _dict;
                }
                return _dict;
            }
        }

        public static IEnumerable<T> Parse<T>(string fileName) where T : class
        {
            var path = Path.Combine(Environment.CurrentDirectory, AirportsConstants.DataFolder + fileName);
            List<T> valueList = new List<T>();

            int i = 0;
            Console.WriteLine($"{typeof(T)}: {i}");

            using (var reader = new StreamReader(path))
            {
                bool firstRow = true;
                string[] columns = null;

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.AirportSplit(',');

                    if (firstRow)
                    {
                        columns = values;
                        firstRow = false;
                        continue;
                    }

                    var instance = CreateObject<T>(columns, values);
                    if (instance != null
                        && Validator.TryValidateObject(instance, new ValidationContext(instance), new List<ValidationResult>(), true)
                        && !valueList.Contains(instance))
                    {
                        valueList.Add(instance);

                        WriteConsole<T>(++i);
                    }
                }
            }

            Dictionary.Add(typeof(T), valueList.Distinct());

            return valueList;
        }

        private static void WriteConsole<T>(int i) where T : class
        {
            Console.CursorLeft = 0;
            Console.CursorTop = Console.CursorTop > 1 ? Console.CursorTop - 1 : 0;
            Console.WriteLine($"{typeof(T)}: {i}");
        }

        private static T CreateObject<T>(string[] columnNames, string[] columns) where T : class
        {
            var instance = Activator.CreateInstance(typeof(T));
            var type = instance.GetType();
            var columnArr = new Dictionary<string, string>();

            for (int i = 0; i < columnNames.Length; i++)
            {
                columnArr.Add(columnNames[i], columns[i]);
            }

            try
            {
                foreach (var element in columnArr)
                {
                    var prop = GetColumnName(type, element.Key);
                    if (prop != null)
                    {
                        if (prop.PropertyType == typeof(TimeSpan))
                        {
                            TimeSpan ts;
                            TimeSpan.TryParse(element.Value.Trim('"'), out ts);
                            prop.SetValue(instance, ts);
                        }
                        else
                        {
                            prop.SetValue(instance, Convert.ChangeType(element.Value.Trim('"'), prop.PropertyType));
                        }
                    }
                }

                CreateObjectMapping(instance, columnArr);
            }
            catch (InvalidCastException ex)
            {
                var logger = LogManager.GetCurrentClassLogger();
                logger.Error(ex);
                return null;
            }

            return instance as T;
        }

        private static PropertyInfo GetColumnName(Type type, string columnName)
        {
            PropertyInfo property = null;

            foreach (var prop in type.GetProperties())
            {
                var attributes = prop.CustomAttributes.SingleOrDefault(a => a.AttributeType == typeof(ColumnAttribute) &&
                a.ConstructorArguments.Count(ar => ar.Value.ToString().ToLower() == columnName.ToLower()) > 0);

                if (attributes != null || (attributes == null && prop.Name.ToLower() == columnName.ToLower()))
                {
                    property = prop;
                }
            }

            return property;
        }

        private static void CreateObjectMapping(object instance, Dictionary<string, string> columnArr)
        {
            var type = instance.GetType();

            if (type == typeof(City))
            {
                IEnumerable<object> countryObjs = null;
                Dictionary.TryGetValue(typeof(Country), out countryObjs);
                var tmpList = Enumerable.Cast<Country>(countryObjs).OrderBy(c => c.Name);
                var country = Enumerable.Cast<Country>(countryObjs).SingleOrDefault(c => c.Name == columnArr["countryName"]);
                (instance as City).CountryId = country.Id;
                (instance as City).Country = country;
            }
            else if (type == typeof(Airport))
            {
                IEnumerable<object> cityObjs = null;
                Dictionary.TryGetValue(typeof(City), out cityObjs);
                var city = Enumerable.Cast<City>(cityObjs).SingleOrDefault(c => c.Name == columnArr["cityName"]
                                                                             && c.Country.Name == columnArr["countryName"]);
                IEnumerable<object> locationObjs = null;
                Dictionary.TryGetValue(typeof(Location), out locationObjs);
                var location = Enumerable.Cast<Location>(locationObjs).Single(l => l.Altitude == double.Parse(columnArr["altitude"])
                                                                                && l.Latitude == double.Parse(columnArr["latitude"])
                                                                                && l.Longitude == double.Parse(columnArr["longitude"]));
                (instance as Airport).City = city;
                (instance as Airport).CityId = city.Id;
                (instance as Airport).Country = city.Country;
                (instance as Airport).CountryId = city.CountryId;
                (instance as Airport).Location = location;
            }
            else if (type == typeof(Segment))
            {
                IEnumerable<object> airportObjs = null;
                Dictionary.TryGetValue(typeof(Airport), out airportObjs);
                var airports = Enumerable.Cast<Airport>(airportObjs);
                var departureAirport = airports.SingleOrDefault(a => a.Id == double.Parse(columnArr["departureAriport"]));
                var arrivalAirport = airports.SingleOrDefault(a => a.Id == double.Parse(columnArr["arrivalAriport"]));
                IEnumerable<object> airlineObjs = null;
                Dictionary.TryGetValue(typeof(Airline), out airlineObjs);
                var airline = Enumerable.Cast<Airline>(airlineObjs).SingleOrDefault(a => a.Id == int.Parse(columnArr["airline"]));
                (instance as Segment).ArrivalAirport = arrivalAirport;
                (instance as Segment).ArrivalAirportId = arrivalAirport.Id;
                (instance as Segment).DepartureAirport = departureAirport;
                (instance as Segment).DepartureAirportId = departureAirport.Id;
                (instance as Segment).Airline = airline;
                (instance as Segment).AirlineId = airline.Id;
            }
        }
    }
}
