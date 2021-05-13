using FInSearchAPI.Interfaces;
using FinSearchDataAccessLibrary.Models.Database;
using Newtonsoft.Json;
using RestSharp;
using System.Collections.Generic; 

namespace FInSearchAPI.Models.Responses
{
        public class FigiArrayResponse 
    {
            [JsonProperty("data")]
            public List<Figi> Data { get; set; }

            [JsonProperty("error")]
            public string Error { get; set; }
        
    }
}
