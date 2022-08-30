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
                public int Position { get; set; }
                public decimal Priceamount { get; set; }

                public DateTime PricePosTimeStamp { get; set; }

                public override string ToString()
                {

                    return String.Format("{0}\n" + "{1}\n", Position, Priceamount);
                }
            }
        }


    }
    























