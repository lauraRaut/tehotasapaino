using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using static System.Net.Http.HttpClient;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Serialization;
using static Tehotasapaino.Models.DayAHeadPriceAPIClient;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Collections;

namespace Tehotasapaino.Models
{


    public class DayAHeadPriceAPIClient
    {
        // Creating a generic HTTP get call that takes in the url (https://transparency.entsoe.eu/api? 
        // & the individual security token Sampsa requested from the service provider
        // & a dictionary, a collection of key-value pairs we want to search from the database xml. E.g. : "OutBiddingZone_Domain", "10YFI-1--------U" which represents the Price area:Finland)

        public static async Task<string> HttpGetRequestForPrices(string urlName, string securityToken, IDictionary<string, string> @params)
        {
            // We establish a new UriBuilder which allows us to sort of build the url piece by piece for our HttpRequest

            var builder = new UriBuilder(urlName);
            var query = HttpUtility.ParseQueryString(builder.Query);

            // We go through the method parameter collection of key-value pairs is added into the url 
            // The url will look something like this "?DocumentType=A65&ProcessType=A16&OutBiddingZone_Domain=10YAT-APG------L&TimeInterval=2017-05-01T00%3a00Z%2f2017-05-01T03%3a00Z&securityToken=0ea14323-e50b-4de2-8810-05ee1c84dd06"

            foreach (var entry in @params) query[entry.Key] = entry.Value;
            query["securityToken"] = securityToken; builder.Query = query.ToString();
            var url = builder.ToString();

            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = client.GetAsync(url).Result)
                {
                   //Getting the api response, which is in xml format

                    using (HttpContent content = response.Content)
                    {
                        var xml = content.ReadAsStringAsync().Result;
                        return xml;
                    }

                }
                }
            }

        }
    }







