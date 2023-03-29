using Drink.API.Clients;
using System.Runtime.CompilerServices;

namespace Drink.API.Extensions
{
    public static class HttpClientExtensions
    {
        public static IServiceCollection AddHttpClients(this IServiceCollection services, IConfiguration config)
        {
            services.AddHttpClient<IContentClient, ContentClient>()
                    .ConfigureHttpClient(client =>
                    {
                        client.BaseAddress = new Uri(config.GetConnectionString("SpoonacularUrl")!);
                    });       
                    
            return services;
        }
    }
}
