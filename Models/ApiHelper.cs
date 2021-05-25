using FInSearchAPI.Interfaces;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace FInSearchAPI.Models
{
    public   class ApiHelper  : IApiHelper
    {
        #region Fields
        private readonly RestClient restClient;
        private readonly HttpClient httpClient;

        private readonly Dictionary<string,string> headers = new Dictionary<string, string>(){
                          {"json","appication/json" }
                        , {"json-ld","appication/ld+json" }
                        , {"html","text/html" }
                        , {"xml","application/xhtml+xml" }
                        , {"form-data","multipart/form-data" }
        };

        public RestClient RestClient => restClient;

        public HttpClient HttpClient => httpClient;


        #endregion
        #region Constructors
        public ApiHelper() {
   
            httpClient = new HttpClient();
            restClient = new RestClient();
            initializeClients();                 
        }

        #endregion
        #region Methods 
        private void initializeClients(){

            //RestClient
            restClient.AddDefaultHeaders(headers);

            //httpClient
            foreach (var item in headers)
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(item.Value));

  
        }

        #endregion
       
    }
}
