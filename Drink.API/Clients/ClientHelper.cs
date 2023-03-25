using Drink.API;
using Drink.API.Infrastructure;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Net.Http.Headers;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Drink.API.Clients
{
    public static class ClientHelper
    {
        public static async Task<HttpResponseMessage> WithLogs(
            Func<Task<HttpResponseMessage>> action,
            OperationType operation,
            string request)
        {
            var response = await action();
            var content = await response.Content.ReadAsStringAsync();

            response.EnsureSuccessStatusCode();

            return response;
        }

        public static HttpRequestMessage CreateHttpRequest(
            OperationType operation,
            HttpMethod httpMethod,
            string url,
            Config config,
            params object[] fields)
        {
            var httpRequest = new HttpRequestMessage(httpMethod, url);
            httpRequest.Headers.Accept.Clear();
            httpRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return httpRequest;
        }

        public static string SetPath(OperationType operation, Config config, object[] fields)
        {
            switch (operation)
            {
                case OperationType.GetRecipes:
                    return string.Format(config.ApiUris.RandomRecipes, config.SpoonacularApiKey);

                case OperationType.GetWines:
                    return string.Format(
                        string.Concat(config.ApiUris.WinePairing, config.SpoonacularApiKey),
                        fields);

                case OperationType.GetBeers:
                    return string.Format(config.ApiUris.GetBeers, fields);

                case OperationType.GetCocktails:
                    return config.ApiUris.GetCocktails;

                default:
                    return string.Empty;
            }
        }
    }
}
