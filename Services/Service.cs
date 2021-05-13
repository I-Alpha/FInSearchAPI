using FInSearchAPI.Interfaces;
using FinSearchDataAccessLibrary.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks; 
using JsonLD.Entities;
using FinSearchDataAccessLibrary.Handlers;
using FinSearchDataAccessLibrary.Models;
using System.Collections.Generic;

namespace FInSearchAPI.Services
{
    public abstract class Service : IService
    {
        public string Type { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        protected virtual IEntityHandler EntityHandler { get; set; } = new EntityHandler();

        public Entity  CreateObjectFromJson(string json)

        {
            // Parse the response body.            
            var JOentity = JObject.Parse(json);
            var JOEntitytype = JOentity["@type"].ToString();
            var EntityType = EntityHandler.GetEntityType(JOEntitytype);
            JsonLdSerializer JsonLdSerializer = new JsonLdSerializer();
            JsonReader jsonReader = JOentity.CreateReader();

            return (Entity) JsonLdSerializer.Deserialize(jsonReader, EntityType);
         
        } 
        
    }
}