namespace Kramerica.BankID.Authentication.Models
{
    public class CompletionData
    {
        public User user { get; set; }
        public Device device { get; set; }
        public Cert cert { get; set; }
        public string signature { get; set; }
        public string ocspResponse { get; set; }
    }

    public class User
    {
        public string personalNumber { get; set; }
        public string name { get; set; }
        public string givenName { get; set; }
        public string surname { get; set; }
    }

    public class Device
    {
        public string ipAddress { get; set; }
    }

    public class Cert
    {
        public string notBefore { get; set; }
        public string notAfter { get; set; }
    }


}