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
using static active_directory_aspnetcore_webapp_openidconnect_v2.Models.ApiHelper;
using static active_directory_aspnetcore_webapp_openidconnect_v2.Models.DayAheadPrice;
using Newtonsoft.Json.Linq;
using System.Xml;
using System.Globalization;

namespace active_directory_aspnetcore_webapp_openidconnect_v2.Models
{
    public class PriceProcessor
    {

        public static void GetPricesPerSearch()
        {
           var today = DateTime.Today;
           var todayFormatted = today.ToString("yyyy-MM-dd");
           var tomorrow = today.AddDays(1);
           var tomorrowFormatted = tomorrow.ToString("yyyy-MM-dd");

            // Adding the key-value pairs I want to look up in my httpcall.
            IDictionary<string, string> searchterms = new Dictionary<string, string>();
            searchterms.Add("DocumentType", "A44");
            searchterms.Add("in_Domain", "10YFI-1--------U");
            searchterms.Add("out_Domain", "10YFI-1--------U");
            searchterms.Add("TimeInterval", todayFormatted +"T00:00Z/"+ tomorrowFormatted +"T03:00Z");

            var res = ApiHelper.HttpGetRequestForPrices("https://web-api.tp.entsoe.eu/api?", "0ea14323-e50b-4de2-8810-05ee1c84dd06", searchterms);
            if (res.IsCompletedSuccessfully)
            {
                HourandPriceKeyValuePairs(res.Result.ToString());
            }

        }



        public static void HourandPriceKeyValuePairs(string xmlFromApi)
        {
            
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
                            Console.WriteLine($"{startDate.AddHours(hour - 1)} {price/1000} e/kWh");
                        }
                    }
                }

                /*List<JObject> list = jsonFromApi
                .Descendants() // From the JObject we created earlier, we get all of its descendant JTokens
                .Where(jt => jt.Type == JTokenType.Property && ((JProperty)jt).Value.HasValues) // Filter that list to only JProperties whose values have children
                .Cast<JProperty>() // Cast the JTokens to JProperties to make them easier to work with in the next step
                .Select(prop => //Now, for each JProperty we selected, transform it as follows:
                {
                    var obj = new JObject(new JProperty("Point", prop.Name)); //Create a new JObject and add the JProperty Pointproperty of the new object
                    if (prop.Value.Type == JTokenType.Array)
                    {
                        var items = prop.Value.Children<JObject>()
                                              .SelectMany(jo => jo.Properties())
                                              .Where(jp => jp.Value.Type == JTokenType.String);
                        obj.Add(items);
                    }

                    var parentName = prop.Ancestors()
                                        .Where(jt => jt.Type == JTokenType.Property)
                                      .Select(jt => ((JProperty)jt).Name)
                                      .FirstOrDefault();
                    //obj.Add("Parent", parentName ?? "");
                    return obj;
                })
                .ToList();

                Console.WriteLine(list);*/
            }


        }



    }
}
