using Drink.API;
using Drink.API.Infrastructure;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Drink.API.Clients
{
    public class ContentClient : IContentClient
    {
        private static readonly JsonSerializerSettings JsonSettings = new()
        {
            NullValueHandling = NullValueHandling.Ignore
        };

        public ContentClient(
            IHttpClientFactory httpclientFactory,
            IOptionsMonitor<Config> configAccessor)
        {
            client = httpclientFactory.CreateClient("ContentClient");
            config = configAccessor.CurrentValue;
        }

        private readonly HttpClient client;
        private readonly Config config;

        public async Task<T> SendGetAsync<T>(OperationType operation, params object[] fields)
        {
            return await SendAsync<T>(operation, HttpMethod.Get, null, fields);
        }

        public async Task<T> SendPostAsync<T>(OperationType operation, object request, params object[] fields)
        {
            return await SendAsync<T>(operation, HttpMethod.Post, request, fields);
        }

        private async Task<T> SendAsync<T>(
            OperationType operation,
            HttpMethod httpMethod,
            object request,
            params object[] fields)
        {
            var url = GetUrlFromOperation(operation, fields);
            var httpRequest = ClientHelper.CreateHttpRequest(operation, httpMethod, url, config, fields);
            var requestContent = url;

            if (httpMethod == HttpMethod.Post)
            {
                requestContent = JsonConvert.SerializeObject(request, JsonSettings);
                httpRequest.Content = new StringContent(requestContent, Encoding.UTF8, "application/json");
            }

            var response = await ClientHelper.WithLogs(() => client.SendAsync(httpRequest), operation, requestContent);

            return await ProcessResponse<T>(response);
        }

        private static async Task<T> ProcessResponse<T>(HttpResponseMessage response)
        {
            if (AllowedErrorStatusCode(response))
            {
                return default;
            }

            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException($"Response with status {response.StatusCode} and body {await response.Content.ReadAsStringAsync()}");
            }

            var stringJsonResponse = await response.Content.ReadAsStringAsync();
            var deserializedObject = JsonConvert.DeserializeObject<T>(stringJsonResponse);

            if (deserializedObject == null)
            {
                throw new ArgumentException($"The deserializedObject is null in {response.RequestMessage.RequestUri.LocalPath}");
            }

            return deserializedObject;
        }

        public static bool AllowedErrorStatusCode(HttpResponseMessage response)
            => (int)response.StatusCode >= 300 && (int)response.StatusCode < 500;

        private string GetUrlFromOperation(OperationType operation, object[] fields)
        {
            return ClientHelper.SetPath(operation, config, fields);
        }
    }
}
