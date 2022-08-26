using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace Tehotasapaino.Models
{

    public class DayAheadPrice
    {
     

            public class Point
            {
                public int position { get; set; }
                public decimal priceamount { get; set; }

                public DateTime dateRange { get; set; }

                public override string ToString()
                {

                    return String.Format("{0}\n" + "{1}\n", position, priceamount);
                }
            }
        }


    }
    























