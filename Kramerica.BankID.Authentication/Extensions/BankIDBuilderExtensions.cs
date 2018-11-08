using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Kramerica.BankID.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Kramerica.BankID.Authentication.Extensions
{
public static class BankIDBuilderExtensions
{
    public static AuthenticationBuilder AddBankIDAuthentication(this AuthenticationBuilder builder, Action<BankIDAuthenticationOptions> configureOptions)
    {
        return builder.AddScheme<BankIDAuthenticationOptions, BankIDAuthenticationHandler>(
            Kramerica.BankID.Authentication.BankIDAuthenticationDefaults.AuthenticationScheme,
            configureOptions);
    }

}
}