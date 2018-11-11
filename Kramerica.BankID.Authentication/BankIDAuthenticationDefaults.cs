using System.Security;

namespace Kramerica.BankID.Authentication
{
    public static class BankIDAuthenticationDefaults
    {
        public const string AuthenticationScheme = "BankID";
        public const string BANKID_TEST_BASE_URL = "https://appapi2.test.bankid.com/rp/v5";
        public static string OrderRefPropertyName = "BankIDOrderRef";
        public static string PersonalNumberPropertyName = "BankIDPersonalNumber";
        public static string SignatureClaimName = "Signature";
        public static string DeviceIPAddressClaimName = "DeviceIPAddress";
        public static string BankIDIssuer = "BankID";
    }


}