using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using WEB_253503_Gudoryan.BlazorWasm;
using WEB_253503_Gudoryan.BlazorWasm.Services.DataService;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var apiUri = builder.Configuration["ApiUri"];
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiUri) });

builder.Services.AddScoped<IDataService, DataService>();

builder.Services.AddOidcAuthentication(options =>
{
    builder.Configuration.Bind("Keycloak", options.ProviderOptions);
}) ;

await builder.Build().RunAsync();
