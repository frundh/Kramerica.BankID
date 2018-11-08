using System.Net.Http;
using System.Security;
using System.Security.Cryptography.X509Certificates;

namespace Kramerica.BankID.Authentication.Helpers
{
    public static class Certificates
    {
        public const string BANKID_TEST_CERTIFICATE_DOWNLOAD_URL = "https://www.bankid.com/assets/bankid/rp/FPTestcert2_20150818_102329.pfx";

        //Download the TestCertificate directly from BankID. In prodcution the BankID certificate are typically stored in Windows Certificate Store
        //and the process running this application are allowed access to the private key. Here is a helper method trying to simplify test scenarios
        //where the certificate can be downloaded directly from BankID.  
        public static X509Certificate2 DownloadBankIDTestCertificate()
        {
            var testcertpassword = new SecureString();
            testcertpassword.AppendChar('q');
            testcertpassword.AppendChar('w');
            testcertpassword.AppendChar('e');
            testcertpassword.AppendChar('r');
            testcertpassword.AppendChar('t');
            testcertpassword.AppendChar('y');
            testcertpassword.AppendChar('1');
            testcertpassword.AppendChar('2');
            testcertpassword.AppendChar('3');
            return new X509Certificate2(
                        new HttpClient().GetByteArrayAsync(BANKID_TEST_CERTIFICATE_DOWNLOAD_URL).Result, 
                        testcertpassword);

        }
    } 
}