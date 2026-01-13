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
    var configuration = sp.GetRequiredService<IConfiguration>();
    var apiUrl = configuration["ApiBaseUrl"];
    
    Console.WriteLine($"🔧 Configuring HttpClient with API URL: {apiUrl}");
    
    // Use relative URL if not specified (same origin)
    if (string.IsNullOrEmpty(apiUrl))
    {
        apiUrl = builder.HostEnvironment.BaseAddress;
        Console.WriteLine($"⚠️ No API URL configured, using base address: {apiUrl}");
    }
    
    httpClient.BaseAddress = new Uri(apiUrl);
    Console.WriteLine($"✅ HttpClient configured with base address: {httpClient.BaseAddress}");
    return httpClient;
});

// Register services
builder.Services.AddScoped<ApiService>();
builder.Services.AddScoped<RegionStateService>();

await builder.Build().RunAsync();
