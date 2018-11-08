using Newtonsoft.Json;

namespace Kramerica.BankID.Authentication.Models
{
    public class BankIDAuthRequest
    {
        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public string personalNumber { get; set; }

        public string endUserIp {get;set;}

        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public string Requirement { get; set; }
    }
}