using AuxiliaryService.API.AdContract;
using AuxiliaryService.API.Shared.Integration.Rest;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace AuxiliaryService.ApplicationServices.Shared.Integration.Rest
{
    public class RestClient : IRestClient
    {

        #region .ctor

        public RestClient() {}

        #endregion

        #region private methods

        private void Authorization(HttpClient client, RestClientConfiguration configuration)
        {
            if (configuration.AuthType == AuthType.Basic)
            {
                AdContract.NotNullNorEmpty(configuration.UserName);
                AdContract.NotNullNorEmpty(configuration.Password);

                client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Basic",
                    Convert.ToBase64String(
                        System.Text.ASCIIEncoding.ASCII.GetBytes(
                            string.Format("{0}:{1}", configuration.UserName, configuration.Password))));

            }

            if (configuration.AuthType == AuthType.BearerToken)
            {
                AdContract.NotNullNorEmpty(configuration.Token);

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", configuration.Token);
            }
        }

        private HttpClient CreateHttpClient(RestClientConfiguration configuration)
        {
            HttpClient httpClient = null;

            if (configuration.DeflateGzipDecompression)
            {
                var handler = new HttpClientHandler();
                handler.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                httpClient = new HttpClient(handler);
            }
            else
            {
                httpClient = new HttpClient();
            }

            return httpClient;
        }

        #endregion 

        #region IRestClient

        public async Task<string> PostAsync(string message, RestClientConfiguration configuration)
        {

            AdContract.NotNullNorEmpty(message);
            AdContract.NotNull(configuration);
            AdContract.NotNullNorEmpty(configuration.Uri);
            AdContract.NotNullNorEmpty(configuration.MediaType);

            using (var client = CreateHttpClient(configuration))
            {
                Authorization(client, configuration);

                var restMsg = new StringContent(message, Encoding.UTF8, configuration.MediaType);
                var response = await client.PostAsync(configuration.Uri, restMsg).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    var resultContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return resultContent;

                }
                else
                {
                    throw new Exception(string.Format("Error while post request: {0} ({1})", (int)response.StatusCode, response.ReasonPhrase));
                }
            }
        }

        public async Task<string> PostStreamAsync(Stream stream, string filename, RestClientConfiguration configuration)
        {

            using (var client = CreateHttpClient(configuration))
            {
                Authorization(client, configuration);

                if (!stream.CanRead)
                    throw new Exception("Stream can not be read");

                stream.Position = 0;
                var streamContent = new StreamContent(stream);

                var response = await client.PostAsync(configuration.Uri, streamContent).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    var resultContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return resultContent;
                }
                else
                {
                    throw new Exception(string.Format("Error while post request: {0} ({1})", (int)response.StatusCode, response.ReasonPhrase));
                }
            }

        }

        public async Task DeleteAsync(RestClientConfiguration configuration)
        {
            AdContract.NotNull(configuration);
            AdContract.NotNullNorEmpty(configuration.Uri);

            using (var client = CreateHttpClient(configuration))
            {
                Authorization(client, configuration);

                var response = await client.DeleteAsync(configuration.Uri).ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(string.Format("Error while post request: {0} ({1})", (int)response.StatusCode, response.ReasonPhrase));
                }

            }
        }

        public async Task<Stream> GetStreamAsync(RestClientConfiguration configuration)
        {
            AdContract.NotNull(configuration);
            AdContract.NotNullNorEmpty(configuration.Uri);

            using (var client = CreateHttpClient(configuration))
            {
                Authorization(client, configuration);

                var response = await client.GetAsync(configuration.Uri).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
                }
                else
                {
                    throw new Exception(string.Format("Error while get request(stream): {0} ({1})", (int)response.StatusCode, response.ReasonPhrase));
                }
            }
        }

        public async Task<string> GetStringAsync(RestClientConfiguration configuration)
        {
            AdContract.NotNull(configuration);
            AdContract.NotNullNorEmpty(configuration.Uri);

            using (var client = CreateHttpClient(configuration))
            {
                Authorization(client, configuration);

                var response = await client.GetAsync(configuration.Uri).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    var str = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return str;
                }
                else
                {
                    throw new Exception(string.Format("Error while get request(string): {0} ({1})", (int)response.StatusCode, response.ReasonPhrase));
                }
            }
        }


        #endregion

    }
}
