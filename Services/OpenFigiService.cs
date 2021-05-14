using FInSearchAPI.Models;
using FInSearchAPI.Models.Requests; 
using FInSearchAPI.Models.Responses;
using FinSearchDataAccessLibrary;
using FinSearchDataAccessLibrary.Handlers;
using FinSearchDataAccessLibrary.Interfaces; 
using FinSearchDataAccessLibrary.Models.Database;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static FInSearchAPI.Models.Request;

namespace FInSearchAPI.Services
{
    public class OpenFigiService :  Service
    { 

        public int Id { set; get; }
         

        private static new IEntityHandler EntityHandler = new EntityHandler();

        private const string ApiBaseUrl = "https://api.openfigi.com";

        private  readonly RestClient apiClient;

        private IJsonSerializer JsonSerializer = new NewtonsoftJsonSerializer(); 

        public Dictionary<string, string> v3ApiUrls = new Dictionary<string, string> {
                                                        { "map", "v3/mapping" },
                                                        { "mapbykey" , "v3/mapping/:key" },
                                                        { "search" , "v3/search"},
                                                        { "filter" , "v3/filter" }
                                                    }; 

        public string getParamString(string type) => v3ApiUrls[type];
        public OpenFigiService(ApiHelper _ApiHelper)
        {
            apiClient = _ApiHelper.RestClient; 
            apiClient.BaseUrl = new Uri(ApiBaseUrl); 
        }

        public async Task<FigiRequest> ConvertCompanyToFigiRequest(Company _company) {
            return new FigiRequest 
            {
                IdType = _company.PrimaryQuote.ExchangeCode,
                IdValue = _company.PrimaryQuote.Ticker
            } ;
        }
     
        public async Task<string> SearchAsync(List<FigiRequest> ObjRequest)
        {
            /*
*//*               var keyValueContent =  Utilities.ToKeyValue(ObjRequest);*//*
   //  var jsonfigi = ObjRequest.ToKeyValue();
   var settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
   var json2 = JsonConvert.SerializeObject(ObjRequest, settings);

   var content = new StringContent(json2, Encoding.UTF8, "application/json"); 

  // var formUrlEncodedContent = new FormUrlEncodedContent(json2.ToKeyValue());
//   formUrlEncodedContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

   HttpResponseMessage response = apiClient.PostAsync(getParamString("search"), content).Result;

  // HttpResponseMessage response2 = apiClient.PostAsync(getParamString("search"), formUrlEncodedContent).Result;

   if (response.IsSuccessStatusCode)
   {
       // Parse the response body.
       var json = await response.Content.ReadAsStringAsync();
    //   var json3 = await response2.Content.ReadAsStringAsync();
       return json;

   }
   else
   {
       Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
       return "fail ";

   }
*/

            return "";
        }

        public async Task<IRestResponse<List<FigiArrayResponse>>> MapAsync(List<FigiRequest> ObjRequest)
        {
            apiClient.BaseUrl = new Uri("https://api.openfigi.com/v3/mapping");
            var request = new RestRequest(Method.POST);
             
            request.AddHeader("Content-Type", "text/json");
            request.AddJsonBody(ObjRequest);
            request.JsonSerializer = JsonSerializer;
            var response = apiClient.Post<List<FigiArrayResponse>>(request);          

            if (response.IsSuccessful)
            {
                // Parse the response body.
                return response; 
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.StatusDescription);
                return new RestResponse<List<FigiArrayResponse>>();

            }

        }

    }

}
