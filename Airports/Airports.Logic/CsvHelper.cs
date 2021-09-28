using Airports.Logic.Entities;
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
        public static IEnumerable<T> Parse<T>(string fileName) where T : class
        {
            var path = Path.Combine(Environment.CurrentDirectory, AirportsConstants.DataFolder + fileName);
            List<T> valueList = new List<T>();

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
                    if (instance != null && Validator.TryValidateObject(instance, new ValidationContext(instance), new List<ValidationResult>(), true))
                    {
                        valueList.Add(instance);
                    }
                }
            }

            return valueList;
        }

        private static T CreateObject<T>(string[] columnNames, string[] columns) where T : class
        {
            var instance = Activator.CreateInstance(typeof(T));
            var type = instance.GetType();

            try
            {
                for (int i = 0; i < columnNames.Length; i++)
                {
                    var prop = GetColumnName(type, columnNames[i]);
                    if (prop != null)
                    {
                        if (prop.PropertyType == typeof(TimeSpan))
                        {
                            TimeSpan ts;
                            TimeSpan.TryParse(columns[i].Trim('"'), out ts);
                            prop.SetValue(instance, ts);
                        }
                        else
                        {
                            prop.SetValue(instance, Convert.ChangeType(columns[i].Trim('"'), prop.PropertyType));
                        }
                    }
                }
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

                if (attributes != null || prop.Name.ToLower() == columnName.ToLower())
                {
                    property = prop;
                }
            }

            return property;
        }
    }
}
