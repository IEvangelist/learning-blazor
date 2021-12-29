// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Extensions;

internal static class WebAssemblyHostBuilderExtensions
{
    internal static WebAssemblyHostBuilder ConfigureServices(
        this WebAssemblyHostBuilder builder)
    {
        var (services, configuration) =
            (builder.Services, builder.Configuration);
;
        services.AddScoped<ApiAccessAuthorizationMessageHandler>();
        services.Configure<WebApiOptions>(
            configuration.GetSection(nameof(WebApiOptions)));

        static WebApiOptions? GetWebApiOptions(IServiceProvider serviceProvider) =>
            serviceProvider.GetService<IOptions<WebApiOptions>>()
                ?.Value;

        var addHttpClient =
            IHttpClientBuilder (
                string httpClientName,
                Func<WebApiOptions?, string?> webApiOptionsUrlFactory) =>
                services.AddHttpClient(
            httpClientName, (serviceProvider, client) =>
            {
                var options = GetWebApiOptions(serviceProvider);
                var apiUrl = webApiOptionsUrlFactory(options);
                if (apiUrl is { Length: > 0 })
                    client.BaseAddress = new Uri(apiUrl);

                var cultureService = serviceProvider.GetRequiredService<CultureService>();

                client.DefaultRequestHeaders.AcceptLanguage.ParseAdd(
                    cultureService.CurrentCulture.TwoLetterISOLanguageName);
            })
            .AddHttpMessageHandler<ApiAccessAuthorizationMessageHandler>();

        _= addHttpClient(
            HttpClientNames.ServerApi,
            options => options?.WebApiServerUrl);
        _ = addHttpClient(
            HttpClientNames.PwnedServerApi,
            options => options?.PwnedWebApiServerUrl);

        services.AddScoped(
            sp => sp.GetRequiredService<IHttpClientFactory>()
                .CreateClient(HttpClientNames.ServerApi));
        services.AddLocalization();
        services.AddMsalAuthentication(
            options =>
            {
                configuration.Bind("AzureAdB2C", options.ProviderOptions.Authentication);
                options.ProviderOptions.LoginMode = "redirect";
                var add = options.ProviderOptions.DefaultAccessTokenScopes.Add;

                add("openid");
                add("offline_access");
                add(AzureAuthenticationTenant.ScopeUrl);
            });

        services.AddOptions();
        services.AddAuthorizationCore();
        services.AddScoped<SharedHubConnection>();
        services.AddSingleton<AppInMemoryState>();
        services.AddSingleton<CultureService>();
        services.AddSingleton(typeof(CoalescingStringLocalizer<>));
        services.AddScoped
            <IWeatherStringFormatterService, WeatherStringFormatterService>();
        services.AddScoped<GeoLocationService>();
        services.AddHttpClient<GeoLocationService>(client =>
        {
            var apiHost = "https://api.bigdatacloud.net";
            var reverseGeocodeClientRoute = "data/reverse-geocode-client";
            client.BaseAddress =
                new Uri($"{apiHost}/{reverseGeocodeClientRoute}");
            client.DefaultRequestHeaders.AcceptEncoding.ParseAdd("gzip");
        });
        services.AddJokeServices();
        services.AddLocalStorage();
        services.AddSingleton<IJSInProcessRuntime>(
            provider =>
                (IJSInProcessRuntime)provider.GetRequiredService<IJSRuntime>());

        return builder;
    }
}
