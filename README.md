# Kramerica.BankID
OpenID Connect server using the Swedish BankID as identification. This is built as a proof-of-concept without any real implementation in mind.

The full specification is NOT implemented especially regarding messages with users.

It uses the excellent [AspNet.Security.OpenIdConnect.Server (ASOS) framework](https://github.com/aspnet-contrib/AspNet.Security.OpenIdConnect.Server)

## BankID
BankID is the Swedish de-facto standard digital identity created jointly by the Swedish banks. Techically it is an OOB (out-of-band) identification solution where both the device, containing the identity, and the RP communicates on the backend with a central JSON REST API. Earlier versions (pre v5) used a SOAP API.

As no secure information (such as passwords) are entered in the loginpage, BankID is actually pretty secure to implement directly in local login forms. But OIDC offers the benefits of being well supported on almost any platform.

In september 2018 they added support for QR-codes making 

So far this project has two parts:

* Kramerica.BankID.Authentication
Here we have an AspNet Core AuthenticationHandler and a typed HttpClient for use with the IHttpClientFactory

* Kramerica.BankID.OIDCServer
