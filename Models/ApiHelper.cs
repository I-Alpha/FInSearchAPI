using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace FInSearchAPI.Models
{
    public   class ApiHelper
    {

        public readonly RestClient RestClient ;
        public readonly HttpClient HttpClient ;

        public ApiHelper() {

            initializeClients();                    
            HttpClient = new HttpClient();
            RestClient = new RestClient();
        }

        private readonly Dictionary<string,string> headers = new Dictionary<string, string>(){
                          {"json","appication/json" }
                        , {"json-ld","appication/ld+json" }
                        , {"html","text/html" }
                        , {"xml","application/xhtml+xml" }
                        , {"form-data","multipart/form-data" }
        };

        private void initializeClients(){

            //RestClient
            RestClient.AddDefaultHeaders(headers);

            //httpClient
            foreach (var item in headers) 
              HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(item.Value));

  
        }
    
}
}
