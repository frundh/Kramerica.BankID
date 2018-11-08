using Microsoft.AspNetCore.Authentication;

namespace Kramerica.BankID.Authentication
{
    public class BankIDAuthenticationOptions : AuthenticationSchemeOptions
    {
        public int CollectIntervalInMilliseconds { get; set; }
        public int CollectTimeoutInMilliseconds {get;set;}
    }
}