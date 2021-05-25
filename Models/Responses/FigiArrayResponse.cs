using FInSearchAPI.Interfaces;
using FinSearchDataAccessLibrary.Models.Database;
using Newtonsoft.Json;
using RestSharp;
using System.Collections.Generic;

namespace FInSearchAPI.Models.Responses
{
    public class FigiArrayResponse : RestResponse
    {
        public FigiArrayResponse() 
        {
        }

        [JsonProperty("error")]
        public string Error { get; set; }

        [JsonProperty("data")]
        public List<FigiInstrument> Data { get; set; }
    } 
}
