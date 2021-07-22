// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Globalization;
using System.Net.Http;
using Learning.Blazor;
using Learning.Blazor.Extensions;
using Learning.Blazor.LocalStorage;
using Learning.Blazor.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

const string ServerApi = nameof(ServerApi);

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");

if (builder.HostEnvironment.IsDevelopment())
{
    builder.Logging.SetMinimumLevel(LogLevel.Debug);
}

var serverUrl = ConfigureServices(
    builder.Services,
    builder.Configuration,
    builder.HostEnvironment);

await using var host = builder.Build();

var factory =
    host.Services.GetRequiredService<ILoggerFactory>();
var logger = factory.CreateLogger("Learning.Blazor.Program");
logger.LogInformation("Using Web API server url: {Url}", serverUrl);

var localStorage = host.Services.GetRequiredService<ILocalStorage>();
var clientCulture = await localStorage.GetAsync<string>(StorageKeys.ClientCulture);
if (clientCulture is not null)
{
    CultureInfo culture = new(clientCulture);
    CultureInfo.DefaultThreadCurrentCulture = culture;
    CultureInfo.DefaultThreadCurrentUICulture = culture;
}

await host.RunAsync();

static string ConfigureServices(
    IServiceCollection services,
    IConfiguration configuration,
    IWebAssemblyHostEnvironment hostEnvironment)
{
    var serverUrl = configuration.GetValue<string>("WebApiServerUrl");

    services.AddHttpClient(ServerApi, client => client.BaseAddress = new Uri(serverUrl));
    services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient(ServerApi));

    services.AddSingleton<CultureService>();
    services.AddTransient<TemperatureUnitConversionService>();
    services.AddTransient<SpeedUnitConversionService>();
    services.AddTransient(typeof(IWeatherStringFormatterService<>), typeof(WeatherStringFormatterService<>));

    services.AddTwitterComponent(configuration);
    services.AddLocalStorage();

    services.AddLocalization();

    services.AddMsalAuthentication(
        options => configuration.Bind("AzureAdB2C", options.ProviderOptions.Authentication));

    return serverUrl;
}
