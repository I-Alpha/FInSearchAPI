using FinSearchDataAccessLibrary.Models.Database;
using System;
using System.Collections.Generic;

namespace FInSearchAPI.Models.Responses
{
    public class CompanyLevelInfo
    {         
        public string PermId {get; set;}
        public bool Public  {get;set;}
        public DateTime IPODate { get; set; }
        public string LEI { get; set; }
        public string PrimaryIndustry { get; set; }
        public string PrimaryEconomic { get; set; }
        public string BusinessSector { get; set; }
        public string DomiciledIn { get; set; }
        public string Website { get; set; }
        public string Fullticker { get; set; }
        public Quote PrimaryQuote { get; set; }
        public CompanyLevelInfo(Company company)
        {
            PermId = company.PermId ;
            Public = (company.IPODate.Date <= DateTime.Now.Date) ? true : false;
            IPODate = company.IPODate ;
            LEI = company.LEI ?? "n/a";
            PrimaryIndustry = company.PrimaryIndustryGroup ?? "n/a";
            PrimaryEconomic = company.PrimaryEconomicSector ?? "n/a";
            BusinessSector = company.PrimaryBusinessSector ?? "n/a";
            DomiciledIn = company.DomiciledIn ?? "n/a";
            Website = company.URL ?? "n/a";
            Fullticker = company.FullTicker ?? "n/a";
            PrimaryQuote = company.PrimaryQuote;
        }
    }
}