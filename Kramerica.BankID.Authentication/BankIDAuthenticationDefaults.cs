using System.Security;

namespace Kramerica.BankID.Authentication
{
public static class BankIDAuthenticationDefaults
{
    public const string AuthenticationScheme = "BankID";
    public const string BankIDIdentifierFormFieldName = "personalNumber";
    public const string BANKID_TEST_BASE_URL = "https://appapi2.test.bankid.com/rp/v5";
}


}