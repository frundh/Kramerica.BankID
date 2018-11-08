using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Kramerica.BankID.Authentication.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
//using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;

namespace Kramerica.BankID.Authentication
{
    public class BankIDClient
    {
        private HttpClient _client;
        private ILogger<BankIDClient> _logger;

        public BankIDClient(HttpClient client, ILogger<BankIDClient> logger, IConfiguration config)
        {
            _client = client;
            _logger = logger;
        }

        public async Task<string> Auth(string personalNumber, string endUserIP)
        {
            try
            {

                var authRequest = new StringContent(JsonConvert.SerializeObject(new BankIDAuthRequest
                {
                    personalNumber = personalNumber,
                    endUserIp = endUserIP
                }), Encoding.UTF8, "application/json");

                //BankID does NOT accept Charset on the Content-Type header
                authRequest.Headers.ContentType.CharSet = String.Empty;

                var authHttpResponse = await _client.PostAsync(
                    "auth",
                    authRequest);

                authHttpResponse.EnsureSuccessStatusCode();

                var authResponse = await authHttpResponse.Content.ReadAsAsync<BankIDAuthResponse>();

                return authResponse.orderRef;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred interacting with BankID Auth API {ex.ToString()}");
                throw;
            }
        }

        public async Task<BankIDCollectResponse> Collect(string orderRef)
        {
            try
            {

                var collectRequest = new StringContent(JsonConvert.SerializeObject(new BankIDCollectRequest
                {
                    orderRef = orderRef
                }), Encoding.UTF8, "application/json");

                //BankID does NOT accept Charset on the Content-Type header
                collectRequest.Headers.ContentType.CharSet = String.Empty;

                var collectHttpResponse = await _client.PostAsync(
                    "collect",
                    collectRequest);

                collectHttpResponse.EnsureSuccessStatusCode();

                var collectResponse = await collectHttpResponse.Content.ReadAsAsync<BankIDCollectResponse>();

                return collectResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred interacting with BankID Collect API {ex.ToString()}");
                throw;
            }

        }

    }
}