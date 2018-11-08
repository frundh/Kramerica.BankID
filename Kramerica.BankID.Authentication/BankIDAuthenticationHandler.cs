using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Kramerica.BankID.Authentication.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

namespace Kramerica.BankID.Authentication
{
    public class BankIDAuthenticationHandler : AuthenticationHandler<BankIDAuthenticationOptions>
    {
        private BankIDClient _bankIDClient;

        public BankIDAuthenticationHandler(
            IOptionsMonitor<BankIDAuthenticationOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            BankIDClient bankIDClient) : base(options, logger, encoder, clock)
        {
            _bankIDClient = bankIDClient;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            StringValues personalNumberFormPostField;
            BankIDCollectResponse collectResponse;
            string collectStatus = string.Empty;

            if (!Request.Form.TryGetValue(BankIDAuthenticationDefaults.BankIDIdentifierFormFieldName, out personalNumberFormPostField))
            {
                return AuthenticateResult.Fail(new ArgumentException("{BankIDAuthenticationDefaults.BankIDIdentifier} was not found in the Form Post"));
            }

            try
            {
                var orderRef = await _bankIDClient.Auth(personalNumberFormPostField[0], Context.Connection.RemoteIpAddress.ToString());

                var collectRequest = new BankIDCollectRequest
                {
                    orderRef = orderRef
                };

                var collectStopWatch = new Stopwatch();
                collectStopWatch.Start();

                do
                {
                    System.Threading.Tasks.Task.Delay(Options.CollectIntervalInMilliseconds).Wait();

                    collectResponse = await _bankIDClient.Collect(orderRef);
                    collectStatus = collectResponse.status;

                    if (collectStopWatch.ElapsedMilliseconds > Options.CollectTimeoutInMilliseconds)
                    {
                        return AuthenticateResult.Fail(new TimeoutException($"A successful BankID interaction never completed withing the timeout set. The latest Collect status was : {collectStatus}"));
                    }
                }
                while (collectStatus != "complete");

                collectStopWatch.Stop();


            }
            catch (Exception ex)
            {
                return AuthenticateResult.Fail($"Exception while doing BankID verification. Message {ex.ToString()}");
            }

            var ticket = await CreateAuthenticationticketAsync(collectResponse);
            return AuthenticateResult.Success(ticket);

        }

        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            throw new NotImplementedException();
        }

        public async Task<AuthenticationTicket> CreateAuthenticationticketAsync(BankIDCollectResponse collectResponse)
        {
            return new AuthenticationTicket(
                new ClaimsPrincipal(
                    new ClaimsIdentity(
                        new List<Claim>()
                        {
                                // new Claim(ClaimTypes.Sid, collectResponse.completionData.user.personalNumber),
                                new Claim(ClaimTypes.NameIdentifier, collectResponse.completionData.user.personalNumber),
                                new Claim(ClaimTypes.Name, collectResponse.completionData.user.name),
                        }, Scheme.Name)),
                Scheme.Name);
        }

    }

}