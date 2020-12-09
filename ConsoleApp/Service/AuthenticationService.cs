using IdentityModel.Client;
using NLog;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace ConsoleApp.Service
{
    class AuthenticationService
    {
        ILogger logger;
        public AuthenticationService()
        {
            logger = LogManager.GetCurrentClassLogger();
        }
        public async void GetUser(ulong imei)
        {
            var clientAuth = new System.Net.Http.HttpClient();
            var disco = await clientAuth.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = "http://127.0.0.1.xip.io",
                Policy =
            {
                RequireHttps = false
            }
            });

            if (disco.IsError)
            {
                logger.Error(disco.Error);
                return;
            }

            // request token
            var tokenResponse = await clientAuth.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = "DeviceIdentity",
                Scope = "BridgeAPI"
            });

            if (tokenResponse.IsError)
            {
                logger.Error(tokenResponse.Error);
                return;
            }
            logger.Info(tokenResponse.Json);

            // call api
            var baseAddress = new Uri("https://localhost:5006");
            var httpClient = WebRequest.CreateHttp(baseAddress);
            httpClient.KeepAlive = false;
            httpClient.Method = "POST";
            httpClient.ContentType = "application/json";
            httpClient.Headers.Add("Authorization", $"Bearer {tokenResponse.AccessToken}");
            var requestStream = httpClient.GetRequestStream();
            requestStream.Write(Encoding.UTF8.GetBytes(imei.ToString()));
            requestStream.Flush();
            var response = new StreamReader(((HttpWebResponse)httpClient.GetResponse()).GetResponseStream()).ReadToEnd();


        }
    }
}
