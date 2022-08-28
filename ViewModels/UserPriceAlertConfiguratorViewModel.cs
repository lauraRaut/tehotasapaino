using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Graph;
using static Tehotasapaino.Models.DayAheadPrice;

namespace Tehotasapaino.Models
{
    public class UserPriceAlertConfiguratorViewModel
    {
        public LoggedInPerson loggedInPerson { get; set; }
        public DayAHeadPriceData dayAHeadPriceData { get; set; }
        public UserPriceAlertConfiguratorViewModel(User userFromAzureAD, bool isInPriceAlertBetaProgram, List<Point> priceList) 
        {
            this.loggedInPerson = new LoggedInPerson(userFromAzureAD, isInPriceAlertBetaProgram);
            this.dayAHeadPriceData = new DayAHeadPriceData(priceList);
        }

        
       
        public class LoggedInPerson
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }            
            public bool isInPriceAlertBetaProgram { get; set; }

            public LoggedInPerson(User userFromAzureAD, bool isPriceInPriceAlertProgram)
            {
                this.FirstName = userFromAzureAD.GivenName;
                this.LastName = userFromAzureAD.Surname;
                this.Email = userFromAzureAD.Mail;
                this.isInPriceAlertBetaProgram = isPriceInPriceAlertProgram;
            }
        }

        public class DayAHeadPriceData 
        {

            public List<Point> DayAheadPrices { get; set; } = new List<Point>();

            public DayAHeadPriceData(List<Point> dayAheadPrice) 
            {
                this.DayAheadPrices = dayAheadPrice.Where(x => x.PricePosTimeStamp >= DateTime.Now.AddHours(-1)).Take(24).ToList();
            }

        }
    }
}