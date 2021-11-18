// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using Learning.Blazor.Handlers;
using Learning.Blazor.Localization;
using Learning.Blazor.Options;
using Learning.Blazor.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Options;

namespace Learning.Blazor.Extensions;

internal static class WebAssemblyHostBuilderExtensions
{
    internal static WebAssemblyHostBuilder ConfigureServices(this WebAssemblyHostBuilder builder)
    {
        var services = builder.Services;
        var configuration = builder.Configuration;

        services.AddScoped<ApiAccessAuthorizationMessageHandler>();

        services.Configure<WebApiOptions>(
            configuration.GetSection(nameof(WebApiOptions)));

        static WebApiOptions? GetWebApiOptions(IServiceProvider serviceProvider) =>
            serviceProvider.GetService<IOptions<WebApiOptions>>()
                ?.Value;

        services.AddHttpClient(
            HttpClientNames.ServerApi, (serviceProvider, client) =>
            {
                var options = GetWebApiOptions(serviceProvider);
                if (options is { WebApiServerUrl: { Length: > 0 } })
                    client.BaseAddress = new Uri(options.WebApiServerUrl);

                var cultureService = serviceProvider.GetRequiredService<CultureService>();

                client.DefaultRequestHeaders.AcceptLanguage.ParseAdd(
                    cultureService.CurrentCulture.TwoLetterISOLanguageName);
            })
            .AddHttpMessageHandler<ApiAccessAuthorizationMessageHandler>();

        services.AddHttpClient(
            HttpClientNames.PwnedServerApi, (serviceProvider, client) =>
            {
                var options = GetWebApiOptions(serviceProvider);
                if (options is { PwnedWebApiServerUrl: { Length: > 0 } })
                    client.BaseAddress = new Uri(options.PwnedWebApiServerUrl);

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
                    AzureAuthenticationTenant.ScopeUrl);
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
            typeof(IWeatherStringFormatterService<>),
            typeof(WeatherStringFormatterService<>));

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
