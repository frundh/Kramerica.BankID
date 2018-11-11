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
using Microsoft.AspNetCore.Http;
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

            //Get the personalNumber from the request
            string orderRef = (string)Request.HttpContext.Items[BankIDAuthenticationDefaults.OrderRefPropertyName];
            string personalNumber = (string)Request.HttpContext.Items[BankIDAuthenticationDefaults.PersonalNumberPropertyName];

            try
            {
                //Without either an personalNumber and orderRef there is not much to do here
                if (orderRef == null && personalNumber == null)
                {
                    return AuthenticateResult.Fail(new ArgumentException($"Neither {BankIDAuthenticationDefaults.OrderRefPropertyName} or {BankIDAuthenticationDefaults.PersonalNumberPropertyName} was not found in the request properties which is needed to do the BankID auth"));
                }

                if (orderRef == null)
                {
                    //Get the orrderRef number that will be used for the subsequent calls to Collect 
                    var authResponse = await _bankIDClient.Auth(personalNumber, Context.Connection.RemoteIpAddress.ToString());
                    orderRef = authResponse.orderRef;
                }

                //Create stopwatch to handle the max amount of time we want to wait for the collect
                //and how long time the used should be given to finialize the BankID identification
                var collectStopWatch = new Stopwatch();
                collectStopWatch.Start();

                do
                {
                    //The BankID specification states that one can not call Collect "to often". Typically this should
                    //be something like 2000ms
                    System.Threading.Tasks.Task.Delay(Options.CollectIntervalInMilliseconds).Wait();

                    //Call BankID Collect API
                    collectResponse = await _bankIDClient.Collect(orderRef);

                    //The status id
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
                                new Claim(ClaimTypes.NameIdentifier, collectResponse.completionData.user.personalNumber),
                                new Claim(ClaimTypes.Name, collectResponse.completionData.user.name),
                                new Claim(BankIDAuthenticationDefaults.SignatureClaimName, collectResponse.completionData.signature, ClaimValueTypes.String, BankIDAuthenticationDefaults.BankIDIssuer),
                                new Claim(BankIDAuthenticationDefaults.DeviceIPAddressClaimName, collectResponse.completionData.device.ipAddress, ClaimValueTypes.String, BankIDAuthenticationDefaults.BankIDIssuer)
                        }, Scheme.Name)),
                Scheme.Name);
        }

    }

}