# Kramerica.BankID
A proof-of-concept .NET Core OpenID Connect server for BankID identification

It uses the excellent [AspNet.Security.OpenIdConnect.Server (ASOS) framework](https://github.com/aspnet-contrib/AspNet.Security.OpenIdConnect.Server)


BankID is the Swedish de-facto standard digital identity solution created by jointly by the banks. Techically it uses OOB (out-of-band) identification with a central JSON REST API. 

So far two parts. More to 

* Kramerica.BankID.Authentication
* Kramerica.BankID.OIDCServer
