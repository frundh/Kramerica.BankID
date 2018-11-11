using System.Collections.Generic;
using Kramerica.BankID.OIDCServer.Models;

namespace Kramerica.BankID.OIDCServer.Config
{
    public class Configuration
    {
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientID = "testclient",
                    DisplayName = "A first test",
                    RedirectUri = "https://openidconnect.net/callback",
                    LogoutRedirectUri = "",
                    Secret = "supersecret"
                },
                
                                new Client
                {
                    ClientID = "mvc-sample",
                    DisplayName = "A simple sample",
                    RedirectUri = "https://localhost:5003/signin-oidc",
                    LogoutRedirectUri = "",
                    Secret = "supersecret"
                }
            };
        }
    }
}