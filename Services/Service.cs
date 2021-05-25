using FInSearchAPI.Interfaces;
using FinSearchDataAccessLibrary.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks; 
using JsonLD.Entities;
using FinSearchDataAccessLibrary.Handlers;
using FinSearchDataAccessLibrary.Models;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using FinSearchDataAccessLibrary.Models.Database;

namespace FInSearchAPI.Services
{
    public abstract class Service :  IService<Service>
    {
        #region Fields
        public string Type { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public abstract ILogger Logger { get; set; }
        protected static IEntityHandler EntityHandler { get; set; } = new EntityHandler();
        #endregion

        #region Methods
        public static Entity  CreateObjectFromJson(string json)

        { 
            // Parse the response body.            
            var JOentity = JObject.Parse(json);
            var JOEntitytype = JOentity["@type"].ToString();
            var EntityType = EntityHandler.GetEntityType(JOEntitytype);
            JsonLdSerializer JsonLdSerializer = new JsonLdSerializer();
            JsonReader jsonReader = JOentity.CreateReader();

            return (Entity) JsonLdSerializer.Deserialize(jsonReader, EntityType);
         
        }

        public abstract Task<string> GetEntryAsync(string id);
        public abstract Task<string> SearchAsync(string searchString);
        public abstract Task<string> MatchAsync(Company company);

     

        #endregion
    }
}