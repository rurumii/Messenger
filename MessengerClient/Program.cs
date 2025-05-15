using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MessengerClient.Services;
using Microsoft.AspNetCore.Components.Authorization;

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
            builder.Services.AddScoped<MessageApiService>();
            builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
            builder.Services.AddAuthorizationCore();
            await builder.Build().RunAsync();
        }
    }
}
