# Kramerica.BankID
OpenID Connect server using the Swedish BankID as identification. This is built as a proof-of-concept without any real implementation in mind.

The full RP specification of BankID is NOT implemented especially regarding user messages etc.

The OpenID server uses the excellent [AspNet.Security.OpenIdConnect.Server (ASOS) framework](https://github.com/aspnet-contrib/AspNet.Security.OpenIdConnect.Server)

## BankID
BankID is the Swedish de-facto standard digital identity created jointly by the Swedish banks. Techically it is an OOB (out-of-band) identification solution where both the device, containing the identity, and the RP communicates on the backend with a central JSON REST API (pre version 5 used an SOAP API).

As no secure information (such as passwords) are entered in the loginpage, BankID is actually pretty secure to implement directly in local login forms. But OIDC offers the benefits of being well supported on almost any platform.

In september 2018 BankID added support for QR-codes making security better by require a physical closeness between the identity and the RP-application. For years bad guys have used social hacking to gain access to systems with a BankID requirement.

So far this project has two parts:

* Kramerica.BankID.Authentication
Here we have an AspNet Core AuthenticationHandler and a typed HttpClient for use with the IHttpClientFactory

* Kramerica.BankID.OIDCServer
The ASOS OIDC Server with the BankID identification.


```csharp
           services.AddHttpClient<BankIDClient>(client =>
                {
                    client.BaseAddress = new Uri("https://appapi2.test.bankid.com/rp/v5/");
                    client.DefaultRequestHeaders.Add("Accept", "application/json");
                })
                .ConfigurePrimaryHttpMessageHandler(h => 
                {
                    var handler = new HttpClientHandler();
                    handler.ClientCertificates.Add(Certificates.DownloadBankIDTestCertificate());
                    return handler;
                });

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "ServerCookie";
            })
            
            .AddCookie("ServerCookie", options =>
            {
                options.Cookie.Name = CookieAuthenticationDefaults.CookiePrefix + "ServerCookie";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
                //Choose Signin page with wither classic BankID with manual entry of the civicid (personnummer)
                //or the new BankID QR-code method that is considered more secure
                // options.LoginPath = new PathString("/signin");
                options.LoginPath = new PathString("/signinqr");
                options.LogoutPath = new PathString("/signout");
            })

            .AddBankIDAuthentication(options=> {
                //The BankID is firewall-friendly but that means that polling of the Collect method is required.
                //Something like a 2-3 seconds interval seems like a good compromise
                options.CollectIntervalInMilliseconds = 2000;
                //BankID has a maximum timeout of 30 seconds doing a Collect. Here we can set a lower value if we want to.
                options.CollectTimeoutInMilliseconds=30000;
            })

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

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseAuthentication();
            app.UseStaticFiles();
            app.UseMvc();
            app.UseWelcomePage();
        }
 ```
