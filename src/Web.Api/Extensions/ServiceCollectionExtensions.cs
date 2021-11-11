﻿// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using Learning.Blazor.Api.Http;
using Learning.Blazor.Api.Options;


namespace Learning.Blazor.Api.Extensions;

internal static class ServiceCollectionExtensions
{
    internal static IServiceCollection AddApiServices(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddResponseCaching();
        services.AddHttpClient(); // Adds IHttpClientFactory, untyped and unnamed.

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
