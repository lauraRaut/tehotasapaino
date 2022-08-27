using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.IO;
using System.Net;
using System.Net.Http.Headers;
using static System.Net.Http.HttpClient;
using System.Text;
using System.Text.Json;
using System.Xml.Serialization;
using static Tehotasapaino.Models.ApiHelper;
using static Tehotasapaino.Models.DayAheadPrice;
using Newtonsoft.Json.Linq;
using System.Xml;
using System.Globalization;
using Microsoft.Extensions.Configuration;
using System.Xml.Linq;

namespace Tehotasapaino.Models
{


    public class PriceProcessor
    {
        private readonly IConfiguration Configuration;
        public PriceProcessor() { }


        public List<Point> GetPricesPerSearch()
        {
            //string apikey = this.Configuration["ApiKey:DayAhedPrice"];
            string apikey = "0ea14323-e50b-4de2-8810-05ee1c84dd06";
            DateTime today = DateTime.Today;
            DateTime yesterday = today.AddDays(-1);
            DateTime tomorrow = today.AddDays(1); 

            string yesterdayFormatted = yesterday.ToString("yyyyMMddHHmm", CultureInfo.InvariantCulture);
            string tomorrowFormatted = tomorrow.ToString("yyyyMMddHHmm", CultureInfo.InvariantCulture);

            // Adding the key-value pairs I want to look up in my httpcall.
            IDictionary<string, string> searchterms = new Dictionary<string, string>();
            searchterms.Add("DocumentType", "A44");
            searchterms.Add("in_Domain", "10YFI-1--------U");
            searchterms.Add("out_Domain", "10YFI-1--------U");
            searchterms.Add("periodStart", yesterdayFormatted);
            searchterms.Add("periodEnd", tomorrowFormatted);


            var res = ApiHelper.HttpGetRequestForPrices("https://web-api.tp.entsoe.eu/api?", apikey, searchterms);
            if (res.IsCompletedSuccessfully)
            {
                return HourandPriceKeyValuePairs(res.Result.ToString());

            }
            return new List<Point>();

        }

        public static List<Point> HourandPriceKeyValuePairs(string xmlFromApi)
        {
            List<Point> prices = new List<Point>();

            XElement docXElem = XElement.Parse(xmlFromApi);
            XNamespace ns2 = RemoveWhitespace(@"urn:iec62325.351:tc57wg16: 451 - 3:publicationdocument: 7:0");
            prices = (from e in docXElem.Elements(ns2 + "TimeSeries").Elements(ns2 + "Period")
                             let startTime = e.Element(ns2 + "timeInterval").Element(ns2 + "start").Value
                             from p in e.Elements(ns2 + "Point")
                             let hour = int.Parse(p.Element(ns2 + "position").Value)
                             let kWhprice = decimal.Parse(p.Element(ns2 + "price.amount").Value, CultureInfo.InvariantCulture)/10
                             select new Point { 
                                 PricePosTimeStamp = DateTime.Parse(startTime).AddHours(hour-1), 
                                 Position = hour, 
                                 Priceamount = Decimal.Round(kWhprice)}).ToList();
            return prices;
        }

        private static string RemoveWhitespace(string input)
        {
            return new string(input.ToCharArray()
                .Where(c => !Char.IsWhiteSpace(c))
                .ToArray());
        }

    }


}
