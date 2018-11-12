# Kramerica.BankID
OpenID Connect server using the Swedish BankID as identification. This is built as a proof-of-concept without any real implementation in mind.

The full RP specification of BankID is NOT implemented especially regarding user messages etc.

The OpenID parts use the excellent [AspNet.Security.OpenIdConnect.Server (ASOS) framework](https://github.com/aspnet-contrib/AspNet.Security.OpenIdConnect.Server)

## BankID
BankID is the Swedish de-facto standard digital identity created jointly by the Swedish banks. Techically it is an OOB (out-of-band) identification solution where both the device, containing the identity, and the RP communicates on the backend with a central JSON REST API (pre version 5 used an SOAP API).

As no secret information (such as pins or passwords) are entered in the loginpage, BankID is actually pretty secure and really simple to implement directly in local login forms. But that is no fun is it? And OIDC offers the benefits of being well supported on almost any platform and provides that nice separation of authentication and application.

In september 2018 BankID added support for QR-codes making security better by require a physical closeness between identity and the 'application'. For years bad guys have used social hacking to fool users giving them access.

So far this project has two parts:

#### Kramerica.BankID.Authentication
An AuthenticationHandler and a typed HttpClient for use with the IHttpClientFactory

#### Kramerica.BankID.OIDCServer
The actual OpenID Connect Server hosting the UI pages etc. Uses ASOS OIDC Server with the BankID AuthenticationHandler (BankIDAuthenticationHandler) as authentication and relevant Singin-views and AuthenticationController modifications

### Example
![Blabla](https://user-images.githubusercontent.com/1846780/48379134-5e564480-e6d3-11e8-9834-c5c4ffe075d1.png)

#### Usage in startup.cs
This is using BankID test
```csharp

//The HttpClientFactory for the typed BankID communication.
services.AddHttpClient<BankIDClient>(client =>
    {
        client.BaseAddress = new Uri("https://appapi2.test.bankid.com/rp/v5/");
        client.DefaultRequestHeaders.Add("Accept", "application/json");
    })
    .ConfigurePrimaryHttpMessageHandler(h => 
    {
        var handler = new HttpClientHandler();
        //Set the client certificate to use against BankID. This is TESTso we will download the certificate on-the-fly. In real-world this would use Certificate Store.
        handler.ClientCertificates.Add(Certificates.DownloadBankIDTestCertificate().Result);
        //BankID test servers certificate are typically not in the trusted root store if you are on Azure. This will bypass. Do NOT use in production!
        handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };                    
        return handler;
    });

services.AddAuthentication(options =>
{
    options.DefaultScheme = "ServerCookie";
})

.AddCookie("ServerCookie", options =>
{
    options.Cookie.Name = CookieAuthenticationDefaults.CookiePrefix + "ServerCookie";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(1);
    //BankID Mode : Normal "old style" or using QR code
    //Choose Signin page with wither classic BankID with manual entry of the civicid (personnummer)
    //or the new BankID QR-code method that is considered more secure
    //options.LoginPath = new PathString("/signin");
    options.LoginPath = new PathString("/signinqr");
    options.LogoutPath = new PathString("/signout");
})

.AddBankIDAuthentication(options=> {
    //The BankID is firewall-friendly but that means that polling of the Collect method is required as no callback
    //is supported. Something like a 2-3 seconds interval seems like a good compromise
    options.CollectIntervalInMilliseconds = 2000;
    //BankID has a maximum timeout of 30 seconds doing a Collect. Here we can set a lower value if we want to.
    options.CollectTimeoutInMilliseconds=30000;
})

//The rest here is standard ASOS studd 
.AddOAuthValidation()

.AddOpenIdConnectServer(options =>
{
    options.ProviderType = typeof(AuthorizationProvider);

    // Enable the authorization, logout, token and userinfo endpoints.
    options.AuthorizationEndpointPath = "/connect/authorize";
    options.LogoutEndpointPath = "/connect/logout";
    options.TokenEndpointPath = "/connect/token";
    options.UserinfoEndpointPath = "/connect/userinfo";
    options.ConfigurationEndpointPath = "/oidc/.well-known/openid-configuration";

    // Note: see AuthorizationController.cs for more
    // information concerning ApplicationCanDisplayErrors.
    options.ApplicationCanDisplayErrors = true;
    options.AllowInsecureHttp = true;

    // Note: to override the default access token format and use JWT, assign AccessTokenHandler:
    //
    options.AccessTokenHandler = new JwtSecurityTokenHandler
    {
        InboundClaimTypeMap = new Dictionary<string, string>(),
        OutboundClaimTypeMap = new Dictionary<string, string>()
    };
    //
    // Note: when using JWT as the access token format, you have to register a signing key.
    //
    // You can register a new ephemeral key, that is discarded when the application shuts down.
    // Tokens signed using this key are automatically invalidated and thus this method
    // should only be used during development:
    //
    options.SigningCredentials.AddEphemeralKey();
    //
    // On production, using a X.509 certificate stored in the machine store is recommended.
    // You can generate a self-signed certificate using Pluralsight's self-cert utility:
    // https://s3.amazonaws.com/pluralsight-free/keith-brown/samples/SelfCert.zip
    //
    // options.SigningCredentials.AddCertificate("7D2A741FE34CC2C7369237A5F2078988E17A6A75");
    //
    // Alternatively, you can also store the certificate as an embedded .pfx resource
    // directly in this assembly or in a file published alongside this project:
    //
    // options.SigningCredentials.AddCertificate(
    //     assembly: typeof(Startup).GetTypeInfo().Assembly,
    //     resource: "Mvc.Server.Certificate.pfx",
    //     password: "Owin.Security.OpenIdConnect.Server");
});
services.AddScoped<AuthorizationProvider>();
services.AddMvc();
services.AddDistributedMemoryCache();

    }
 ```
