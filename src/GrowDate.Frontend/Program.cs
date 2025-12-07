using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using GrowDate.Frontend;
using GrowDate.Frontend.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Configure HttpClient for API calls
builder.Services.AddScoped(sp =>
{
    var httpClient = new HttpClient();
    var apiUrl = builder.Configuration["ApiBaseUrl"];
    
    // Use relative URL if not specified (same origin)
    if (string.IsNullOrEmpty(apiUrl))
    {
        apiUrl = builder.HostEnvironment.BaseAddress;
    }
    
    httpClient.BaseAddress = new Uri(apiUrl);
    return httpClient;
});

// Register services
builder.Services.AddScoped<ApiService>();

await builder.Build().RunAsync();
