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

namespace Tehotasapaino.Models
{


        public class PriceProcessor
        {
            private readonly IConfiguration Configuration;


            public PriceProcessor(IConfiguration configuration)
            {
                Configuration = configuration;
            }
            public PriceProcessor() { }
       

            public List<Point> GetPricesPerSearch()
            {
                //string apikey = this.Configuration["ApiKey:DayAhedPrice"];
                string apikey = "0ea14323-e50b-4de2-8810-05ee1c84dd06";
                var today = DateTime.Today;
                var todayFormatted = today.ToString("yyyy-MM-dd");

                // Adding the key-value pairs I want to look up in my httpcall.
                IDictionary<string, string> searchterms = new Dictionary<string, string>();
                searchterms.Add("DocumentType", "A44");
                searchterms.Add("in_Domain", "10YFI-1--------U");
                searchterms.Add("out_Domain", "10YFI-1--------U");
                searchterms.Add("TimeInterval", todayFormatted + "T00:00Z/" + todayFormatted + "T03:00Z");


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
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlFromApi);
                XmlNamespaceManager nsMgr = new XmlNamespaceManager(doc.NameTable);
                nsMgr.AddNamespace("nm", "urn:iec62325.351:tc57wg16:451-3:publicationdocument:7:0");
                XmlNodeList xnList = doc.SelectNodes("/nm:Publication_MarketDocument/nm:TimeSeries", nsMgr);

                foreach (XmlNode timeSeriesNode in xnList)
                {
                    XmlNodeList periodNodeList = timeSeriesNode.SelectNodes("//nm:Period", nsMgr);
                    foreach (XmlNode periodNode in periodNodeList)
                    {
                        DateTime startDate = DateTime.Parse(periodNode["timeInterval"]["start"].InnerText);
                        foreach (XmlNode pointNode in periodNode.ChildNodes)
                        {
                            if (pointNode.Name == "Point")
                            {
                                int hour = int.Parse(pointNode["position"].InnerText);
                                decimal price = decimal.Parse(pointNode["price.amount"].InnerText, CultureInfo.InvariantCulture);
                                DateTime daterange = startDate.AddHours(hour - 1);
                                Point nextDayPrices = new Point();

                                nextDayPrices.position = hour;
                                nextDayPrices.priceamount = price;
                                nextDayPrices.dateRange = daterange;
                                prices.Add(nextDayPrices);

                            }
                        }
                    }
                }
                return prices;
            }

        }
    
}
