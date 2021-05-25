using FInSearchAPI.Models;
using FInSearchAPI.Models.Requests; 
using FInSearchAPI.Models.Responses;
using FinSearchDataAccessLibrary; 
using FinSearchDataAccessLibrary.Models.Database;
using FinSearchDataAPI;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FInSearchAPI.Services
{
    public class OpenFigiService :  Service
    {

        #region Fields
        public int Id { set; get; }
        public override ILogger Logger { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        private const string ApiBaseUrl = "https://api.openfigi.com";

        private readonly RestClient ApiClient;

        private NewtonsoftJsonSerializer JsonSerializer = new NewtonsoftJsonSerializer();

        public Dictionary<string, string> V3_ApiUrls = new Dictionary<string, string> {
                                                        { "map", "v3/mapping" },
                                                        { "mapbykey" , "v3/mapping/:key" },
                                                        { "search" , "v3/search"},
                                                        { "filter" , "v3/filter" }
                                                    };

        #endregion
        #region Constructors
        public OpenFigiService(ApiHelper _ApiHelper)
        {
            ApiClient = _ApiHelper.RestClient ?? throw new ArgumentNullException(nameof(_ApiHelper));
            ApiClient.BaseUrl = new Uri(ApiBaseUrl);
        }
        #endregion
        #region Methods
        public FigiRequest ConvertCompanyToFigiRequest(Company _company)
        {
            return new FigiRequest("TICKER", _company.PrimaryQuote.Ticker);
        }
        public override async Task<string> SearchAsync(string ObjRequest)
        {
            ApiClient.BaseUrl = new Uri("https://api.openfigi.com/v3/mapping");

            var res = new List<FigiInstrument>();

            var request = new RestRequest(Method.POST)
            {
                JsonSerializer = JsonSerializer,
                RequestFormat = DataFormat.Json
            };

            request.AddHeader("Content-Type", "text/json");
            var obj = JsonConvert.DeserializeObject<FigiRequest[]>(ObjRequest);
            request.AddJsonBody(obj);

            var response = ApiClient.Execute<IList<FigiArrayResponse>>(request);
            if (response.IsSuccessful)
            {
                foreach (FigiArrayResponse item in response.Data)
                {
                    if (item != null && item.Data.Any())
                        return item.Data.Serialize();
                }
            }
            return res.Serialize();
        }
        public  async Task<string> SearchAsync(string ObjRequest, Dictionary<string,string> otherArgs)
        {
            throw new NotImplementedException();
 
        }

        public override async Task<string> GetEntryAsync(string id)
        {
            throw new NotImplementedException(); 
        }

        public override Task<string> MatchAsync(Company company)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
