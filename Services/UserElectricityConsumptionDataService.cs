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
            _logger.LogInformation($"Started parsing CSV stream");
            Dictionary<DateData, List<decimal>> dataPoints = new Dictionary<DateData, List<decimal>>();

            using (StreamReader reader = new StreamReader(fileFromUser.OpenReadStream(),System.Text.Encoding.UTF8))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                string[] format = { "yyyy-MM-dd HH:mm", "dd/MM/yyyy HH.mm" };
                csv.Read();
                csv.ReadHeader();
                while (csv.Read())
                {
                    DateTime dateFromFile;

                    if (!DateTime.TryParseExact(csv.GetField<string>(0), format, CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out dateFromFile))
                    {
                        throw new ArgumentException($"Check date format {csv.GetField<string>(0)}");
                    }
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

        public static int GetWeek(DateTime date)
        {
            Calendar cal = new CultureInfo("fi-FI").Calendar;
            DayOfWeek firstDay = DayOfWeek.Monday;
            CalendarWeekRule rule = CalendarWeekRule.FirstFullWeek;

            return cal.GetWeekOfYear(date, rule, firstDay);
        }

        public static int GetDayOfWeek(DateTime date)
        {
            return (int)date.DayOfWeek;
        }

        public static int GetHour(DateTime date)
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
            
            if (this == compared)
            {
                return true;
            }

            
            if ((compared == null) || !this.GetType().Equals(compared.GetType()))
            {
                return false;
            }
            else
            {
                
                DateData comparedRecord = (DateData)compared;

                
                return this.WeekNum == comparedRecord.WeekNum && this.DayOfWeek == comparedRecord.DayOfWeek && this.Hour == comparedRecord.Hour;
            }
        }

        public override int GetHashCode()
        {
            return Tuple.Create(this.WeekNum, this.DayOfWeek, this.Hour).GetHashCode();
        }
    }
}
