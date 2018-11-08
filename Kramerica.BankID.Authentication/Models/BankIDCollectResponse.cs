using Newtonsoft.Json;

namespace Kramerica.BankID.Authentication.Models
{
    public class BankIDCollectResponse
    {
        public string orderRef { get; set; }
        public string status {get;set;}

        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public string hintCode { get; set; }

        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public CompletionData completionData { get; set; }
    }
}