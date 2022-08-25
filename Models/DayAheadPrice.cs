using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace active_directory_aspnetcore_webapp_openidconnect_v2.Models
{

    public class DayAheadPrice
    {
       
       
        public class PeriodTimeinterval
        {
            public string start { get; set; }
            public string end { get; set; }
        }
      
        public class Period
        {
            public Timeinterval timeInterval { get; set; }
            public string resolution { get; set; }
            public Point[] Point { get; set; }
        }

        public class Timeinterval
        {
            public string start { get; set; }
            public string end { get; set; }
        }

        public class Point
        {
            public int position { get; set; }
            public decimal priceamount { get; set; }

            public DateTime dateRange { get; set; }


            public override string ToString()
            {

                return String.Format("{0}\n" +"{1}\n", position, priceamount);
            }
        }
    }

            
     }
    























