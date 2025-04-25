using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using MessengerClient.Services;

namespace MessengerClient
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddHttpClient("UserService", client =>
            {
                client.BaseAddress = new Uri("https://localhost:7202/");
            });
            builder.Services.AddHttpClient("MessageService", client =>
            {
                client.BaseAddress = new Uri("https://localhost:7130/");
            });
            builder.Services.AddScoped<UserApiService>();
            await builder.Build().RunAsync();
        }
    }
}
