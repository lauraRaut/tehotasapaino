using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace active_directory_aspnetcore_webapp_openidconnect_v2.Models
{

    public class DayAheadPrice
    {
        public class Rootobject
        {
            public Xml xml { get; set; }
            public Publication_Marketdocument Publication_MarketDocument { get; set; }
        }

        public class Xml
        {
            public string version { get; set; }
            public string encoding { get; set; }
        }

        public class Publication_Marketdocument
        {
            public string xmlns { get; set; }
            public string mRID { get; set; }
            public string revisionNumber { get; set; }
            public string type { get; set; }
            public Sender_MarketparticipantMrid sender_MarketParticipantmRID { get; set; }
            public string sender_MarketParticipantmarketRoletype { get; set; }
            public Receiver_MarketparticipantMrid receiver_MarketParticipantmRID { get; set; }
            public string receiver_MarketParticipantmarketRoletype { get; set; }
            public DateTime createdDateTime { get; set; }
            public PeriodTimeinterval periodtimeInterval { get; set; }
            public Timesery[] TimeSeries { get; set; }
        }

        public class Sender_MarketparticipantMrid
        {
            public string codingScheme { get; set; }
            public string text { get; set; }
        }

        public class Receiver_MarketparticipantMrid
        {
            public string codingScheme { get; set; }
            public string text { get; set; }
        }

        public class PeriodTimeinterval
        {
            public string start { get; set; }
            public string end { get; set; }
        }

        public class Timesery
        {
            public string mRID { get; set; }
            public string businessType { get; set; }
            public In_DomainMrid in_DomainmRID { get; set; }
            public Out_DomainMrid out_DomainmRID { get; set; }
            public string currency_Unitname { get; set; }
            public string price_Measure_Unitname { get; set; }
            public string curveType { get; set; }
            public Period Period { get; set; }
        }

        public class In_DomainMrid
        {
            public string codingScheme { get; set; }
            public string text { get; set; }
        }

        public class Out_DomainMrid
        {
            public string codingScheme { get; set; }
            public string text { get; set; }
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
            public string position { get; set; }
            public string priceamount { get; set; }
        }

            
     }
    
}






















