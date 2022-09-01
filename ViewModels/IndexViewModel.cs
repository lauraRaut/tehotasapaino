using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using static Tehotasapaino.Models.DayAheadPrice;

namespace Tehotasapaino.Models
{
    public class IndexViewModel
    {
        public LoggedInPerson loggedInPerson { get; set; }
        public DayAHeadPriceData dayAHeadPriceData { get; set; }
        public UserElectricityUsageData userElectricityConsumptionData {get; set;}


        public IndexViewModel(User userFromAzureAD, bool isRegistered, List<Point> priceList, List<UserElectricityConsumptionData> consumptionList) 
        {
            loggedInPerson = new LoggedInPerson(userFromAzureAD, isRegistered);
            dayAHeadPriceData = new DayAHeadPriceData(priceList);
            userElectricityConsumptionData = new UserElectricityUsageData(consumptionList);
        }



        public class LoggedInPerson
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public bool isRegisteredToService { get; set; }

            public LoggedInPerson(User userFromAzureAD, bool isRegistered)
            {
                this.FirstName = userFromAzureAD.GivenName;
                this.LastName = userFromAzureAD.Surname;
                this.Email = userFromAzureAD.Mail;
                this.isRegisteredToService = isRegistered;
            }


        }

        public class DayAHeadPriceData
        {

            public List<Point> DayAheadPrices { get; set; } = new List<Point>();
            public string maxPrice
            {
                get
                {
                    if (this.DayAheadPrices.Any())
                    {
                        return this.DayAheadPrices.Max(x => x.Priceamount).ToString();

                    }
                    return "0";
                }
                set { }
            }
            public string maxPriceTimeStamp
            {
                get
                {
                    if (this.DayAheadPrices.Any())
                    {
                        DateTime startPos = this.DayAheadPrices.Where(x => x.Priceamount == decimal.Parse(this.maxPrice))
                                       .Select(y => y.PricePosTimeStamp).FirstOrDefault();

                        return $"{startPos:HH} - {startPos.AddHours(1):HH}.00";
                    }
                    return "";
                }
                set { }
            }



            public string minPrice
            {
                get
                {
                    if (this.DayAheadPrices.Any())
                    {
                        return this.DayAheadPrices.Min(x => x.Priceamount).ToString();
                    }
                    return "";
                }
                set { }
            }

            public string minPriceTimeStamp
            {
                get
                {
                    if (this.DayAheadPrices.Any())
                    {
                        DateTime startPos = this.DayAheadPrices.Where(x => x.Priceamount == decimal.Parse(this.minPrice))
                        .Select(y => y.PricePosTimeStamp).FirstOrDefault();
                        return $"{startPos:HH} - {startPos.AddHours(1):HH}.00";
                    }
                    return "";
                }
                set { }
            }

            public int averagePrice
            {
                get
                {
                    if (this.DayAheadPrices.Any())
                    {
                        int averagePrice = (Convert.ToInt32(minPrice) + Convert.ToInt32(maxPrice)) / 2;
                        return averagePrice;
                    }
                    return 0;
                }

                set { }

            }

            public DayAHeadPriceData(List<Point> dayAheadPrice)
            {
                if (dayAheadPrice.Any())
                {
                    this.DayAheadPrices = dayAheadPrice.Where(x => x.PricePosTimeStamp >= DateTime.Now.AddHours(-1)).Take(24).ToList();
                }
                else
                {
                this.DayAheadPrices = new List<Point>();
                }

            }

        }

        public class UserElectricityUsageData
        {

            public List<UserElectricityConsumptionData> DayConsumptionList { get; set; } = new List<UserElectricityConsumptionData>();

            public UserElectricityUsageData(List<UserElectricityConsumptionData> consumptionList)
            {

                if (consumptionList.Any())
                {
                this.DayConsumptionList = consumptionList;
                }

                else
                {
                    this.DayConsumptionList = new List<UserElectricityConsumptionData>();
                }
            }
           
          public List<UserElectricityConsumptionData> DayConsumptionListForGraph
            {
                get
                {  
                    DateTime today = DateTime.Now;
                  //  DateTime tomorrow = today.AddDays(1);
                  //TimeSpan twoDays = tomorrow - today;
                    int currentWeek = UserElectricityConsumptionDataService.GetWeek(today);
                    int currentDay = UserElectricityConsumptionDataService.GetDayOfWeek(today);
                    int currentHour = UserElectricityConsumptionDataService.GetHour(today);

                        var DayConsumptionListForGraph = DayConsumptionList.Where(x => x.WeekNum == currentWeek && x.WeekDay == currentDay || x.WeekDay == currentDay + 1  && x.Hour >= currentHour - 1)
                                                 .OrderBy(x => x.Hour)
                                                  .Take(24).ToList();

                    return DayConsumptionListForGraph;
                }

                set { }
            }


            public string TodayConsumptionFigure
            {
                get {

                    DateTime today = DateTime.Now;
                    int currentWeek = UserElectricityConsumptionDataService.GetWeek(today);
                    int currentDay = UserElectricityConsumptionDataService.GetDayOfWeek(today);
                    int currentHour = UserElectricityConsumptionDataService.GetHour(today);
                    decimal sum = 0;

                    var consumptionFigures = DayConsumptionList.Where(x => x.WeekNum == currentWeek && x.WeekDay == currentDay && x.Hour >= currentHour - 1).OrderBy(x => x.Hour)
                            .Select(x => x.AverageConsumptionkWh).ToList();

                    foreach (var figure in consumptionFigures)
                    {
                        sum += figure;
                    }

                    return sum.ToString();
                }

                set { }
            }


        public string TodayConsumptionPrice
        {
            get
            {
                DateTime today = DateTime.Now;
                int currentWeek = UserElectricityConsumptionDataService.GetWeek(today);
                int currentDay = UserElectricityConsumptionDataService.GetDayOfWeek(today);
                int currentHour = UserElectricityConsumptionDataService.GetHour(today);
                var todayConsumptionPrice = 0;

                List<decimal> consumptionFigures = DayConsumptionList.Where(x => x.WeekNum == currentWeek && x.WeekDay == currentDay && x.Hour >= currentHour - 1).OrderBy(x => x.Hour)
                        .Select(x => x.AverageConsumptionkWh).ToList();

                List<int> hours = DayConsumptionList.Where(x => x.WeekNum == currentWeek && x.WeekDay == currentDay && x.Hour >= currentHour - 1).OrderBy(x => x.Hour)
                        .Select(x => x.Hour).ToList();

                    for (int i = 0; i < consumptionFigures.Count; i++)
                    {
                        todayConsumptionPrice = (Convert.ToInt32(consumptionFigures[i]) * hours[i]) / 10;

                    }
                    return todayConsumptionPrice.ToString();
               }

            set { }
        }

    }


}
}