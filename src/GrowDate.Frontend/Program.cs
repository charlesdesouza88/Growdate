using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Logging;
using GrowDate.Frontend;
using GrowDate.Frontend.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    var apiUrl = configuration["ApiBaseUrl"];

    if (string.IsNullOrWhiteSpace(apiUrl))
        apiUrl = builder.HostEnvironment.BaseAddress;

    return new HttpClient { BaseAddress = new Uri(apiUrl) };
});

builder.Logging.SetMinimumLevel(LogLevel.Information);
builder.Services.AddScoped<ApiService>();
builder.Services.AddScoped<RegionStateService>();

await builder.Build().RunAsync();
