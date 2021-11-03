// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using Learning.Blazor.Handlers;
using Learning.Blazor.Localization;
using Learning.Blazor.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace Learning.Blazor.Extensions;

internal static class WebAssemblyHostBuilderExtensions
{
    internal static WebAssemblyHostBuilder ConfigureServices(this WebAssemblyHostBuilder builder)
    {
        var services = builder.Services;
        var configuration = builder.Configuration;

#pragma warning disable IL2026
        // Methods annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code
        // This is a valid use case as it's a primitive type
        var serverUrl = configuration.GetValue<string>("WebApiServerUrl");
        var pwnedServerUrl = configuration.GetValue<string>("PwnedWebApiServerUrl");
#pragma warning restore IL2026

        services.AddScoped<ApiAccessAuthorizationMessageHandler>();

        services.AddHttpClient(
            HttpClientNames.ServerApi, (serviceProvider, client) =>
            {
                client.BaseAddress = new Uri(serverUrl);

                var cultureService = serviceProvider.GetRequiredService<CultureService>();

                client.DefaultRequestHeaders.AcceptLanguage.ParseAdd(
                    cultureService.CurrentCulture.TwoLetterISOLanguageName);
            })
            .AddHttpMessageHandler<ApiAccessAuthorizationMessageHandler>();

        services.AddHttpClient(
            HttpClientNames.PwnedServerApi, (serviceProvider, client) =>
            {
                client.BaseAddress = new Uri(pwnedServerUrl);

                var cultureService = serviceProvider.GetRequiredService<CultureService>();

                client.DefaultRequestHeaders.AcceptLanguage.ParseAdd(
                    cultureService.CurrentCulture.TwoLetterISOLanguageName);
            })
            .AddHttpMessageHandler<ApiAccessAuthorizationMessageHandler>();

        services.AddScoped(
            sp => sp.GetRequiredService<IHttpClientFactory>()
                .CreateClient(HttpClientNames.ServerApi));

        services.AddLocalization();

        services.AddMsalAuthentication(
            options =>
            {
#pragma warning disable IL2026
                // Methods annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code
                // This has been working, I can only assume it will continue to!
                configuration.Bind("AzureAdB2C", options.ProviderOptions.Authentication);
#pragma warning restore IL2026

                options.ProviderOptions.LoginMode = "redirect";

                options.ProviderOptions.DefaultAccessTokenScopes.Add("openid");
                options.ProviderOptions.DefaultAccessTokenScopes.Add("offline_access");
                options.ProviderOptions.DefaultAccessTokenScopes.Add(
                    "https://learningblazor.onmicrosoft.com/ee8868e7-73ad-41f1-88b4-dc698429c8d4/User.ApiAccess");
            });

        services.AddOptions();
        services.AddAuthorizationCore();

        services.AddScoped<SharedHubConnection>();
        services.AddSingleton<AppInMemoryState>();
        services.AddSingleton<CultureService>();
        services.AddSingleton(typeof(CoalescingStringLocalizer<>));
        services.AddScoped<TemperatureUnitConversionService>();
        services.AddScoped<SpeedUnitConversionService>();
        services.AddScoped(
            typeof(IWeatherStringFormatterService<>), typeof(WeatherStringFormatterService<>));

        services.AddScoped<GeoLocationService>();
        services.AddHttpClient<GeoLocationService>(client =>
        {
            client.BaseAddress = new Uri("https://api.bigdatacloud.net/data/reverse-geocode-client");
            client.DefaultRequestHeaders.AcceptEncoding.ParseAdd("gzip");
        });

        services.AddLocalStorage();

        return builder;
    }
}
