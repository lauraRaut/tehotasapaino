using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.IO;
using Microsoft.AspNetCore.Http;
using CsvHelper;
using Microsoft.Extensions.Logging;
using System.Linq;
using Tehotasapaino.Models;


namespace Tehotasapaino.Models
{
    public class UserElectricityConsumptionDataService
    {

        private readonly ILogger<UserElectricityConsumptionDataService> _logger;

        public UserElectricityConsumptionDataService(ILogger<UserElectricityConsumptionDataService> logger)
        {
            _logger = logger;
        }

        public List<UserElectricityConsumptionData> GetUserElectricityWeekDayHourAverages(IFormFile fileFromUser)
        {
            List<UserElectricityConsumptionData> averagedUserConsumption = new List<UserElectricityConsumptionData>();

            _logger.LogInformation($"Creating consumption list");

            averagedUserConsumption = BuildDataAnalysisModelFromCSV(fileFromUser)
                                     .Select(record => new UserElectricityConsumptionData {
                                                                                              WeekNum = record.Key.WeekNum,
                                                                                              WeekDay = record.Key.DayOfWeek,
                                                                                              Hour = record.Key.Hour,
                                                                                              AverageConsumptionkWh = record.Value.Average()
                                                                                           }).ToList();

            return averagedUserConsumption;
        }

        private Dictionary<DateData, List<decimal>> BuildDataAnalysisModelFromCSV(IFormFile fileFromUser)
        {
            //string filepath = @"C:\Users\Sampsa\source\repos\LinqTestailua\LinqTestailua\files\electricity.csv";

            _logger.LogInformation($"Started parsing CSV stream");
            Dictionary<DateData, List<decimal>> dataPoints = new Dictionary<DateData, List<decimal>>();

            using (StreamReader reader = new StreamReader(fileFromUser.OpenReadStream()))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {

                csv.Read();
                csv.ReadHeader();
                while (csv.Read())
                {
                    DateTime dateFromFile = DateTime.Parse(csv.GetField<string>(0));
                    DateData record = new DateData
                    {
                        WeekNum = GetWeek(dateFromFile),
                        DayOfWeek = GetDayOfWeek(dateFromFile),
                        Hour = GetHour(dateFromFile)
                    };

                    decimal kWhConsumption = csv.GetField<decimal>(1);

                    if (dataPoints.ContainsKey(record))
                    {
                        dataPoints[record].Add(kWhConsumption);
                    }
                    else
                    {
                        dataPoints.Add(record, new List<decimal> { kWhConsumption });
                    }
                }
            }
            _logger.LogInformation($"Parsing CSV done");
            return dataPoints;
        }

        private static int GetWeek(DateTime date)
        {
            Calendar cal = new CultureInfo("fi-FI").Calendar;
            DayOfWeek firstDay = DayOfWeek.Monday;
            CalendarWeekRule rule = CalendarWeekRule.FirstFullWeek;

            return cal.GetWeekOfYear(date, rule, firstDay);
        }

        private static int GetDayOfWeek(DateTime date)
        {
            return (int)date.DayOfWeek;
        }

        private static int GetHour(DateTime date)
        {
            return (int)date.Hour;
        }

    }

    public class DateData
    {

        public int WeekNum { get; set; }
        public int DayOfWeek { get; set; }
        public int Hour { get; set; }

        public override bool Equals(object compared)
        {
            // if the variables are located in the same position, they are equal
            if (this == compared)
            {
                return true;
            }

            // if the compared object is null or not of type Book, the objects are not equal
            if ((compared == null) || !this.GetType().Equals(compared.GetType()))
            {
                return false;
            }
            else
            {
                // convert the object to a Book object
                DateData comparedRecord = (DateData)compared;

                // if the values of the object variables are equal, the objects are, too
                return this.WeekNum == comparedRecord.WeekNum && this.DayOfWeek == comparedRecord.DayOfWeek && this.Hour == comparedRecord.Hour;
            }
        }

        public override int GetHashCode()
        {
            return Tuple.Create(this.WeekNum, this.DayOfWeek, this.Hour).GetHashCode();
        }
    }
}
