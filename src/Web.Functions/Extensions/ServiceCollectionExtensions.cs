// Copyright (c) 2021 David Pine. All rights reserved.
//  Licensed under the MIT License.

using Learning.Blazor.Functions.Options;
using Learning.Blazor.Functions.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Learning.Blazor.Functions.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddOpenWeatherMapServices(
            this IServiceCollection services, IConfiguration configuration)
        {
            services.AddLogging()
                .AddOptions()
                .Configure<OpenWeatherMapOptions>(configuration.GetSection(nameof(OpenWeatherMapOptions)))
                .AddSingleton<IWeatherService, OpenWeatherMapService>();

            services.AddHttpClient<OpenWeatherMapService>();

            return services;
        }
    }
}
