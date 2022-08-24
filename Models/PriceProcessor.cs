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


namespace active_directory_aspnetcore_webapp_openidconnect_v2.Models
{
    public class PriceProcessor
{
        public static void GetPricesPerSearch()
        {

            IDictionary<string, string> searchterms = new Dictionary<string, string>();
            searchterms.Add("DocumentType", "A44");
            searchterms.Add("OutBiddingZone_Domain", "10YAT-APG------L");
            searchterms.Add("in_Domain", "10YFI-1--------U");
            searchterms.Add("out_Domain", "10YFI-1--------U");
            searchterms.Add("TimeInterval", "2022-08-22T00:00Z/2022-08-23T03:00Z");

            var res = ApiHelper.HttpGetRequestForPrices("https://web-api.tp.entsoe.eu/api?", "0ea14323-e50b-4de2-8810-05ee1c84dd06", searchterms);
            if (res.IsCompletedSuccessfully)
            {
                Console.WriteLine(res.Result.ToString());
            }

        }




    }



}
