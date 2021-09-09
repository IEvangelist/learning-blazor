// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using Learning.Blazor.Api.Http;
using Learning.Blazor.Api.Options;
using Learning.Blazor.Api.Services;
using Learning.Blazor.Extensions;
using Learning.Blazor.Http.Extensions;

namespace Learning.Blazor.Api.Extensions;

internal static class ServiceCollectionExtensions
{
    internal static IServiceCollection AddApiServices(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddResponseCaching();
        services.AddHttpClient(); // Adds IHttpClientFactory, untyped and unnamed.

        services.AddPwnedServices(
            configuration.GetSection("PwnedOptions"),
            HttpClientBuilderRetryPolicyExtensions.GetDefaultRetryPolicy);

        services.AddStackExchangeRedisCache(
            options => options.Configuration =
                configuration["RedisCacheOptions:ConnectionString"]);

        services.AddHttpClient(HttpClientNames.WebFunctionsClient)
            .AddDefaultTransientHttpErrorPolicy();

        services.AddScoped<WeatherFunctionClientService>();
        services.Configure<WebFunctionsOptions>(
            configuration.GetSection(nameof(WebFunctionsOptions)));

        services.AddSingleton<WeatherLanguageService>();

        services.AddJokeServices(configuration);
        services.AddTwitterServices(configuration);
        services.AddHostedService<TwitterWorkerService>();

        services.AddLocalization();

        return services;
    }
}
