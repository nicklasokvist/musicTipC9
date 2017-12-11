using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using MusicTip171210.Models;
/// <summary>
/// Same as Class from example project with MemoryCache disabled (error not looked into). /Nicklas 171211
/// </summary>
namespace MusicTip171210.SpotifyApiHandler
{
    public class SpotifyAuthClientCredentialsHttpMessageHandler : DelegatingHandler
    {
        private const string AuthenticationEndpoint = "https://accounts.spotify.com/api/token";
        private readonly string _clientId;
        private readonly string _clientSecret;

        public SpotifyAuthClientCredentialsHttpMessageHandler(string clientId, string clientSecret, HttpMessageHandler httpMessageHandler) : base(httpMessageHandler)
        {
            _clientId = clientId;
            _clientSecret = clientSecret;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.Headers.Authorization == null)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await GetAuthenticationTokenAsync());
            }
            return await base.SendAsync(request, cancellationToken);
        }

        private async Task<string> GetAuthenticationTokenAsync()
        {
            var cacheKey = "SpotifyWebApiSession-Token" + _clientId;

            //var token = MemoryCache.Default.Get(cacheKey) as string;
            var token = "";// = MemoryCache.Default.Get(cacheKey) as string;

            //if (token == null)
            if (token == "")
            {
                var timeBeforeRequest = DateTime.Now;

                AuthenticationResponse response = await GetAuthenticationTokenResponse();

                token = response.AccessToken;
                if (token == null)
                    throw new AuthenticationException("Spotify authentication failed");

                var expireTime = timeBeforeRequest.AddSeconds(response.ExpiresIn);

                //MemoryCache.Default.Set(cacheKey, token, new DateTimeOffset(expireTime));
            }
            return token;
        }

        private async Task<AuthenticationResponse> GetAuthenticationTokenResponse()
        {
            var client = new HttpClient();

            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "client_credentials")
                //new KeyValuePair<string, string>("scope", "")
            });

            var authHeader = BuildAuthHeader();

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, AuthenticationEndpoint);
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Basic", authHeader);
            requestMessage.Content = content;

            var response = await client.SendAsync(requestMessage);
            var responseString = await response.Content.ReadAsStringAsync();

            var authenticationResponse = JsonConvert.DeserializeObject<AuthenticationResponse>(responseString);
            return authenticationResponse;
        }
        private string BuildAuthHeader()
        {
            return Base64Encode(_clientId + ":" + _clientSecret);
        }

        private string Base64Encode(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }
    }
}
