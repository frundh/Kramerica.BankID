using Newtonsoft.Json;

namespace Kramerica.BankID.Authentication.Models
{
    public class BankIDAuthResponse
    {
        public string orderRef { get; set; }

        [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
        public string autoStartToken { get; set; }
    }
}