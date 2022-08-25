using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Graph;
using static active_directory_aspnetcore_webapp_openidconnect_v2.Models.DayAheadPrice;

namespace active_directory_aspnetcore_webapp_openidconnect_v2.Models
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


            public DayAHeadPriceData(List<Point> dayAheadPrice) 
            {
                this.DayAheadPrices = dayAheadPrice;
            }

        }
    }
}