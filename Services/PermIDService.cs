using System;
using System.Net.Http;
using System.Threading.Tasks;  
using FinSearchDataAccessLibrary.Interfaces; 
using FinSearchDataAccessLibrary.Models.Database; 
using FInSearchAPI.Models; 
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using RestSharp;
using FinSearchDataAccessLibrary;
using Newtonsoft.Json;
using FinSearchDataAPI;

namespace FInSearchAPI.Services
{

    public class PermIDService : Service
    {

        #region Fields
        private readonly HttpClient HttpClient;
        private readonly RestClient RestClient;

        private NewtonsoftJsonSerializer JsonSerializer = new NewtonsoftJsonSerializer();


        public const string LookUpAPIBaseUrl = "https://permid.org/";
        public const string SearchAPIBaseUrl = "https://api-eit.refinitiv.com/permid/search";

        public const string MatchAPIBaseUrl = SearchAPIBaseUrl + "match";

        private const string APIToken = "access-token=BXCqymrCagItruFZ1V8LppWLY9zXWWpV";
        private const string ResponseFormat = "?format=json-ld";
        private string LookUpAPIUrlParameters { get { return ResponseFormat + "&" + APIToken; } }
        private string SearchAPIUrlParameters { get { return "search?"+APIToken+"&"; }   }

        public override ILogger Logger { get  ; set ; }

        #endregion
        #region Contructors
        public PermIDService(ApiHelper _ApiHelper ,ILogger<PermIDService> _Logger)
        {
            Logger = _Logger ?? throw new ArgumentNullException(nameof(_Logger)); ; 
            HttpClient = _ApiHelper.HttpClient ?? throw new ArgumentNullException(nameof(_ApiHelper));
            RestClient = _ApiHelper.RestClient ?? throw new ArgumentNullException(nameof(_ApiHelper));

          /*  HttpClient.BaseAddress = new Uri("");
            RestClient.BaseUrl = new Uri("");*/
            RestClient.AddDefaultHeader("X-AG-Access-Token".ToLower(), "BXCqymrCagItruFZ1V8LppWLY9zXWWpV");
            HttpClient.DefaultRequestHeaders.Add("X-AG-Access-Token".ToLower(), "BXCqymrCagItruFZ1V8LppWLY9zXWWpV");
        }

        public override async Task<string> SearchAsync(string queryString) {
            throw new NotImplementedException();
        }

        #endregion
        #region Methods
        public  async Task<string>  SearchAsync(string queryString, Dictionary<string,string> otherArgs)
        {
            Logger.LogInformation("Starting Search Async");
            RestClient.BaseUrl = new Uri(SearchAPIBaseUrl);
            var request = new RestRequest(Method.GET)
            {
                JsonSerializer = JsonSerializer,
                RequestFormat = DataFormat.Json

            }; 
            request.AddParameter("q", queryString, ParameterType.QueryString);

            foreach (var (i, v) in otherArgs)
                request.AddParameter(i, v, ParameterType.QueryString);

            var response = RestClient.Execute(request);

            if (response.IsSuccessful)
            {// Parse the response body.

                Logger.LogInformation("Response success : {0}", response);
                return response.Content;
            }
            else
            {

                Logger.LogInformation("Response fail : {0} , code : ", response,response.StatusCode); 
                return "fail ";
            }

        }


        public override async Task<string> MatchAsync(Company company)
        {
            RestClient.BaseUrl= new Uri(MatchAPIBaseUrl);
            Logger.LogInformation("Starting MatchAsync");
            var processedsearchString = MatchAPIBaseUrl;


            var requestbody = new Dictionary<string, string> {
                { "companyName" , company.OrganizationName },
                { "companyPermID" , company.PermId }
            } ;

            var req = "CompanyName,CompanyPermID%0A" + company.OrganizationName + "," + company.PermId;
                  var request = new RestRequest(Method.POST)
            {
                JsonSerializer = JsonSerializer,
                RequestFormat = DataFormat.None
                
            };


            request.AddHeader("Content-Type", "text/plain");
            request.AddHeader("x-openmatch-dataType", "Person");
            request.AddHeader("x-openmatch-numberOfMatchesPerRecord", "100");
            /* request.AddParameter("text/plain",req, ParameterType.RequestBody);*/
            request.AddJsonBody(requestbody);

             var response = RestClient.Execute(request);

            if (response.IsSuccessful)
            {// Parse the response body.

                Logger.LogInformation("Response success : {0}", response);
                return response.Content;
            }
            else
            {

                Logger.LogInformation("Response fail : {0} , code : ", response, response.StatusCode);
                return "fail ";
            }
        }

        public override async Task<string>  GetEntryAsync(string id)
        {   //id = 10 digit permID

            Logger.LogInformation("Starting GetEntryAsync");
            var FullLookUpByIdSUrl = LookUpAPIBaseUrl + transformID(id) + LookUpAPIUrlParameters;

            var response = await HttpClient.GetAsync(FullLookUpByIdSUrl);

            if (response.IsSuccessStatusCode)
            {
                Logger.LogInformation("Response success : {0}", response);
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                Logger.LogInformation("Response fail : {0} , code : ", response, response.StatusCode);
                return "fail";
            }
        }

        private async Task<IEntity> fillQuoteAsync(dynamic thisEntity)
        {
            Logger.LogInformation("Starting GetEntryAsync");
        
                if (thisEntity.GetType() == typeof(Company))
                {
                    if (thisEntity.PrimaryQuote != null)
                    {
                        thisEntity.Quote = await GetPrimaryQuoteAsync(thisEntity);
                    }
                }
           

            
            return thisEntity;
        }
        public async Task<IEntity> GetPrimaryQuoteAsync(dynamic thisEntity)
        {
            Logger.LogInformation("Starting GetPrimaryQuoteAsync with : {0}" , (object)thisEntity);
            var quoteUrl = thisEntity.QuoteURL;
            string quotePermId = ExtractPermIDFromUrl(quoteUrl);
            var json = await GetEntryAsync(quotePermId);
            return CreateObjectFromJson(json);
        }

        private string ExtractPermIDFromUrl(string Url)
        {
            return Url.Split("-")[1];
        }
        public string transformID(string id) => "1-" + id;

        public async Task<List<string>> GetEmployeesForCompanyAsync(Company company)
        {
            /* Uses the vcard: family - name property of the person to find people with “Trump” last name. Then,
                 the tr-person:holdsPosition and tr - person:isPositionIn properties of that person are used to find
                 the organization in which that person has positions.The query is: */

            Logger.LogInformation("Starting GetEmployeesForOrganizationAsync");

            var res = "";

            var searchstring= company.PermId;
            var otherArgs = new Dictionary<string, string>() { { "entitytype", "Person" } };


            if (company != null)
                {  
                    res = await SearchAsync(searchstring, otherArgs);
                   
                }
            return new List<string>() { res};
        }  
        #endregion
    }
}
