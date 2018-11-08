namespace Kramerica.BankID.OIDCServer.Models
{
    public class Client
    {
        public string ClientID { get; set; }
        public string DisplayName { get; set; }
        public string RedirectUri { get; set; }
        public string LogoutRedirectUri { get; set; }
        public string Secret { get; set; }
    }
}