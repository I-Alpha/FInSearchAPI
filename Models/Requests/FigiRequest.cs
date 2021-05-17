
using FinSearchDataAccessLibrary.Models;
using Newtonsoft.Json;

namespace FInSearchAPI.Models.Requests
{
    public class FigiRequest : Entity
    {
        public FigiRequest()
        {

        }

        public FigiRequest(string idType, string idValue)
            : this()
        {
            this.IdType = idType;
            this.IdValue = idValue;
        }

        public FigiRequest WithExchangeCode(string exchCode)
        {
            this.ExchangeCode = exchCode;
            return this;
        }

        public FigiRequest WithMicCode(string micCode)
        {
            this.MicCode = micCode;
            return this;
        }

        public FigiRequest WithCurrency(string currency)
        {
            this.Currency = currency;
            return this;
        }

        public FigiRequest WithMarketSectorDescription(string marketSectorDescription)
        {
            MarketSectorDescription = marketSectorDescription;
            return this;
        }

        [JsonProperty("idType")]
        public string IdType { get; set; }

        [JsonProperty("idValue")]
        public string IdValue { get; set; }

        [JsonProperty("exchCode")]
        public string ExchangeCode { get; set; }

        [JsonProperty("micCode")]
        public string MicCode { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("marketSecDes")]
        public string MarketSectorDescription { get; set; }

        [JsonIgnore]
        public int Id { get; set; }

        [JsonIgnore]
        public string Type { get; set; }

        [JsonIgnore]
        public string BaseUrl { get; set; }

        [JsonIgnore]
        public string Parameters { get; set; }
    }
}



 