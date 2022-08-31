using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Graph;
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
            public bool isRegisteredToService {get;set;}

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
            public string maxPrice {
                get
                {
                    return this.DayAheadPrices.Max(x => x.Priceamount).ToString();
                }
                set { } 
            }
            public string maxPriceTimeStamp
            {
                get
                {
                    DateTime startPos = this.DayAheadPrices.Where(x => x.Priceamount == decimal.Parse(this.maxPrice))
                                       .Select(y => y.PricePosTimeStamp).FirstOrDefault();

                    return $"{startPos:HH} - {startPos.AddHours(1):HH}.00";
                }
                set { }
            }



            public string minPrice
            {
                get
                {
                    return this.DayAheadPrices.Min(x => x.Priceamount).ToString();
                }
                set { }
            }

            public string minPriceTimeStamp
            {
                get
                {
                    DateTime startPos = this.DayAheadPrices.Where(x => x.Priceamount == decimal.Parse(this.minPrice))
                        .Select(y => y.PricePosTimeStamp).FirstOrDefault();
                    return $"{startPos:HH} - {startPos.AddHours(1):HH}.00";
                }
                set { }
            }

            public int averagePrice
            {
                get
                {
                   int averagePrice = (Convert.ToInt32(minPrice) + Convert.ToInt32(maxPrice)) / 2;
                    return averagePrice;
                }

                set { }
                
            }

            public DayAHeadPriceData(List<Point> dayAheadPrice) 
            {
                this.DayAheadPrices = dayAheadPrice.Where(x => x.PricePosTimeStamp >= DateTime.Now.AddHours(-1)).Take(24).ToList();
            }

        }

        public class UserElectricityUsageData
        {

            public List<UserElectricityConsumptionData> DayConsumptionList { get; set; } = new List<UserElectricityConsumptionData>();

            public UserElectricityUsageData(List<UserElectricityConsumptionData> consumptionList)
            {
                this.DayConsumptionList = consumptionList;
            }
           
          public List<decimal> DayConsumptionListForGraph
            {
                get
                { //Use two-day range of days instead of DateTime Now 
                  //use Take 24 , MAYBE  
                    DateTime today = DateTime.Now;
                    int currentWeek = UserElectricityConsumptionDataService.GetWeek(today);
                    int currentDay = UserElectricityConsumptionDataService.GetDayOfWeek(today);
                    int currentHour = UserElectricityConsumptionDataService.GetHour(today);
                            
                     return DayConsumptionList.Where(x => x.WeekNum == currentWeek && x.WeekDay == currentDay && x.Hour >= currentHour-1).OrderBy(x => x.Hour)
                            .Select(x => x.AverageConsumptionkWh).ToList();
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