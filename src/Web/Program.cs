// Copyright (c) 2021 David Pine. All rights reserved.
//  Licensed under the MIT License.

using System;
using System.Net.Http;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Learning.Blazor;
using Learning.Blazor.Extensions;

const string ServerApi = nameof(ServerApi);

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");

ConfigureServices(
    builder.Services,
    builder.Configuration,
    builder.HostEnvironment);

await builder.Build().RunAsync();

static void ConfigureServices(
    IServiceCollection services,
    IConfiguration configuration,
    IWebAssemblyHostEnvironment hostEnvironment)
{
    var serverUrl = configuration["ServerApi"];

    services.AddHttpClient(ServerApi, client => client.BaseAddress = new Uri(serverUrl));
    services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient(ServerApi));

    services.AddTwitterComponent(configuration);
    services.AddMsalAuthentication(options =>
    {
        configuration.Bind("AzureAdB2C", options.ProviderOptions.Authentication);
    });
}
