// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Functions.Extensions;

static class ServiceCollectionExtensions
{
    internal static IServiceCollection AddOpenWeatherMapServices(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddLogging()
            .AddOptions()
            .Configure<OpenWeatherMapOptions>(
                configuration.GetSection(nameof(OpenWeatherMapOptions)))
            .AddSingleton<IWeatherService, OpenWeatherMapService>();

        services.AddHttpClient<OpenWeatherMapService>();

        return services;
    }
}
