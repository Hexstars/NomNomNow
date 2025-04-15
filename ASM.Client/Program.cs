using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ASM.Client;
using Blazored.SessionStorage;
using Microsoft.Extensions.Http; // Add this at the top of the file
var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddBlazoredSessionStorage();

builder.Services.AddHttpClient("AuthHttpClient", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["API:BaseUrl"] ??
                        builder.HostEnvironment.BaseAddress);
})
.AddHttpMessageHandler<CustomAuthHandler>();

builder.Services.AddScoped<CustomAuthHandler>();

await builder.Build().RunAsync();
