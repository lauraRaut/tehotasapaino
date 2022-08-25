using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace active_directory_aspnetcore_webapp_openidconnect_v2.Models
{

    public class DayAheadPrice
    {
     

            public class Point
            {
                public int position { get; set; }
                public decimal priceamount { get; set; }

                public DateTime dateRange { get; set; }

                private readonly IConfiguration Configuration;
                public override string ToString()
                {

                    return String.Format("{0}\n" + "{1}\n", position, priceamount);
                }
            }
        }


    }
    























