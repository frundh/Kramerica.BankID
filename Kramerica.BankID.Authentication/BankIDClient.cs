using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Kramerica.BankID.Authentication.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

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

        public async Task<BankIDAuthResponse> Auth(string personalNumber, string endUserIP)
        {
            try
            {
                //Create the authrequest
                var authRequest = new StringContent(JsonConvert.SerializeObject(new BankIDAuthRequest
                {
                    personalNumber = personalNumber,
                    endUserIp = endUserIP
                }), Encoding.UTF8, "application/json");

                //BankID does NOT accept Charset on the Content-Type header
                authRequest.Headers.ContentType.CharSet = String.Empty;

                //POST BankID Auth API
                var authHttpResponseMessage = await _client.PostAsync(
                    "auth",
                    authRequest);

                authHttpResponseMessage.EnsureSuccessStatusCode();

                var authResponse = await authHttpResponseMessage.Content.ReadAsAsync<BankIDAuthResponse>();

                return authResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error interacting with the BankID Auth API {ex.ToString()}");
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
                _logger.LogError($"Error interacting with the BankID Collect API {ex.ToString()}");
                throw;
            }

        }

    }
}