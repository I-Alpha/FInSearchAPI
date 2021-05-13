﻿using System;
using System.Net.Http;
using System.Threading.Tasks; 
using System.Net.Http.Headers;
using FinSearchDataAccessLibrary.Interfaces;
using FinSearchDataAccessLibrary.Models;
using FinSearchDataAccessLibrary.Models.Database;
using FinSearchDataAccessLibrary.Handlers;

namespace FInSearchAPI.Services
{

    public class PermIDService : Service  
    {
        public int Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        private readonly HttpClient APIClient;

        private static new IEntityHandler EntityHandler = new EntityHandler();

        public const string LookUpAPIBaseUrl = "https://permid.org/";
        public const string SearchAPIBaseUrl = "https://api-eit.refinitiv.com/permid/";

        private const string APIToken = "access-token=BXCqymrCagItruFZ1V8LppWLY9zXWWpV";
        private const string ResponseFormat = "?format=json-ld";
        private string LookUpAPIUrlParameters { get { return ResponseFormat + "&" + APIToken; } }
        private string SearchAPIUrlParameters { get { return SearchAPIBaseUrl + "search?" + "format=json" + "&" + APIToken; } }
      

        public PermIDService(HttpClient _httpClient)
        {
            APIClient= _httpClient; 
            APIClient.DefaultRequestHeaders.Add("X-AG-Access-Token".ToLower(), "BXCqymrCagItruFZ1V8LppWLY9zXWWpV");


        }

        public async Task<string> SearchAsync(string searchString)
        {
            var processedsearchString = SearchAPIUrlParameters + "&q=" + searchString.ToLower();
            HttpResponseMessage response = APIClient.GetAsync(processedsearchString).Result;
            if (response.IsSuccessStatusCode)
            {
                // Parse the response body.
                return await response.Content.ReadAsStringAsync();
                 
           
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                return "fail ";
            }
        }
        public async Task<string> LookUpByIdAsync(string ID)
        {
            if (ID.Length < 10 || ID.Length > 12)
                return "fail";

            if (!(ID[0] == '1' && ID[1] == '-')) ID = "1-" + ID;
            else if ((ID[0] == '-' && ID[1] != '-')) ID = "1" + ID;


            var processedsearchString = LookUpAPIBaseUrl + ID + LookUpAPIUrlParameters;
            HttpResponseMessage response = APIClient.GetAsync(processedsearchString).Result;

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                return "fail";
            }
        }

 /*       public async Task<IEntity> CreateEntityAsync(string json)
        { // Parse the response body.
            ;
            var JOentity = JObject.Parse(json);
            var JOEntitytype = JOentity["@type"].ToString();
            var EntityType =  EntityHandler.GetEntityType(JOEntitytype);

            JsonLdSerializer JsonLdSerializer = new JsonLdSerializer();
            JsonReader jsonReader = JOentity.CreateReader();

            dynamic thisEntity = JsonLdSerializer.Deserialize(jsonReader, EntityType);
            return thisEntity;
        }*/
  
        public async Task<IEntity> fillQuoteAsync(dynamic thisEntity)
        {
            if (thisEntity.GetType() == typeof(Company))
            {
                if (thisEntity.PrimaryQuote != null)
                {
                    thisEntity.Quote = await GetQuoteAsync(thisEntity);
                }
            } 
            return thisEntity;
        }


        public async Task<IEntity> GetQuoteAsync(dynamic thisEntity)
        {
                var quoteUrl = thisEntity.QuoteURL;

                string quotePermId = ExtractPermIDFromUrl(quoteUrl);
                var json = await LookUpByIdAsync(quotePermId);
                return   ((Interfaces.IService)this).CreateObjectFromJson(json);                            
        }

        private string ExtractPermIDFromUrl(string Url)
        {
            return Url.Split("-")[1];
        }
 
    }
}
