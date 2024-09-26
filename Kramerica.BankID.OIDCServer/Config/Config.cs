using System.Collections.Generic;
using Kramerica.BankID.OIDCServer.Models;

namespace Kramerica.BankID.OIDCServer.Config
{
    public class Configuration
    {
        //Instead of classic ASOS that uses an EF context i use something i think fits my needs better.
        //This would be moved to a database, configurationfile or similar in a real scenario.
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
                    ClientID = "authorization-code-client-id",
                    DisplayName = "A simple sample",
                    RedirectUri = "http://app.local.gd:8080/authentication/login-callback",
                    LogoutRedirectUri = "",
                    Secret = "supersecret"
                }
            };
        }
    }
}