// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Api.Extensions;

internal static class ServiceCollectionExtensions
{
    internal static IServiceCollection AddApiServices(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddResponseCaching();
        services.AddHttpClient(); // Adds IHttpClientFactory, untyped and unnamed.

        services.AddMemoryCache();
        //services.AddStackExchangeRedisCache(
        //    options => options.Configuration =
        //        configuration["RedisCacheOptions:ConnectionString"]);

        services.AddSingleton<WeatherLanguageService>();

        services.AddLogicAppClient(
            configuration.GetSection(nameof(LogicAppOptions)));

        // The Twitter API is no longer free for developers testing it out.
        // Considering the switch to supporting Mastadon instead...

        services.AddCosmosDataServices(configuration);

        services.AddLocalization();

        return services;
    }
}
