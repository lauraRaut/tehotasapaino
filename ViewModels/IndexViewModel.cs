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
        public IndexViewModel(User userFromAzureAD, bool isRegistered, List<Point> priceList) 
        {
            loggedInPerson = new LoggedInPerson(userFromAzureAD, isRegistered);
            dayAHeadPriceData = new DayAHeadPriceData(priceList);
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
    }
}