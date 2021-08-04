// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Net.Http;
using Learning.Blazor.Handlers;
using Learning.Blazor.Localization;
using Learning.Blazor.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Learning.Blazor.Extensions
{
    internal static class WebAssemblyHostBuilderExtensions
    {
        const string ServerApi = nameof(ServerApi);

        internal static WebAssemblyHostBuilder ConfigureServices(this WebAssemblyHostBuilder builder)
        {
            var services = builder.Services;
            var configuration = builder.Configuration;

            var serverUrl = configuration.GetValue<string>("WebApiServerUrl");

            services.AddScoped<ApiAccessAuthorizationMessageHandler>();

            services.AddHttpClient(
                ServerApi, (serviceProvider, client) =>
                {
                    client.BaseAddress = new Uri(serverUrl);

                    var cultureService = serviceProvider.GetRequiredService<CultureService>();

                    client.DefaultRequestHeaders.AcceptLanguage.ParseAdd(
                        cultureService.CurrentCulture.TwoLetterISOLanguageName);
                })
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
            services.AddSingleton(typeof(CoalescingStringLocalizer<>));
            services.AddTransient<TemperatureUnitConversionService>();
            services.AddTransient<SpeedUnitConversionService>();
            services.AddTransient(
                typeof(IWeatherStringFormatterService<>), typeof(WeatherStringFormatterService<>));

            services.AddScoped<SharedHubConnection>();

            services.AddTwitterComponent(configuration);
            services.AddLocalStorage();

            services.AddLocalization();

            return builder;
        }
    }
}
