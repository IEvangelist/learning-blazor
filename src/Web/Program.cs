// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Globalization;
using System.Net.Http;
using Learning.Blazor;
using Learning.Blazor.BrowserModels;
using Learning.Blazor.Extensions;
using Learning.Blazor.Handlers;
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

var localStorage = host.Services.GetRequiredService<ILocalStorage>();
var clientCulture = await localStorage.GetAsync<ClientLocalePreference>(StorageKeys.ClientCulture);
if (clientCulture is not null)
{
    try
    {
        CultureInfo culture = CultureInfo.GetCultureInfo(clientCulture.LCID);
        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentUICulture = culture;
    }
    catch (Exception)
    {
        // Can't set the culture I guess.
    }
}

await host.RunAsync();

static string ConfigureServices(
    IServiceCollection services,
    IConfiguration configuration,
    IWebAssemblyHostEnvironment hostEnvironment)
{
    var serverUrl = configuration.GetValue<string>("WebApiServerUrl");

    services.AddScoped<ApiAccessAuthorizationMessageHandler>();

    services.AddHttpClient(
        ServerApi, client => client.BaseAddress = new Uri(serverUrl))
        .AddHttpMessageHandler<ApiAccessAuthorizationMessageHandler>();

    services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient(ServerApi));

    services.AddMsalAuthentication(
        options =>
        {
            configuration.Bind("AzureAdB2C", options.ProviderOptions.Authentication);

                options.ProviderOptions.LoginMode = "redirect";

                options.ProviderOptions.DefaultAccessTokenScopes.Add("openid");
                options.ProviderOptions.DefaultAccessTokenScopes.Add("offline_access");
                options.ProviderOptions.DefaultAccessTokenScopes.Add(
                    "https://learningblazor.onmicrosoft.com/ee8868e7-73ad-41f1-88b4-dc698429c8d4/User.ApiAccess");
        });

    services.AddSingleton<CultureService>();
    services.AddTransient<TemperatureUnitConversionService>();
    services.AddTransient<SpeedUnitConversionService>();
    services.AddTransient(
        typeof(IWeatherStringFormatterService<>), typeof(WeatherStringFormatterService<>));

    services.AddTwitterComponent(configuration);
    services.AddLocalStorage();

    services.AddLocalization();

    return serverUrl;
}
