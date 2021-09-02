// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using Learning.Blazor.Api.Http;
using Learning.Blazor.Api.Options;
using Learning.Blazor.Api.Services;
using Learning.Blazor.Extensions;
using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Retry;

namespace Learning.Blazor.Api.Extensions;

internal static class ServiceCollectionExtensions
{
    internal static IServiceCollection AddApiServices(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddResponseCaching();
        services.AddHttpClient(); // Adds IHttpClientFactory, untyped and unnamed.

        services.AddPwnedServices(
            configuration.GetSection("PwnedOptions"), GetRetryPolicy);

        services.AddStackExchangeRedisCache(
            options => options.Configuration =
                configuration["RedisCacheOptions:ConnectionString"]);

        services.AddHttpClient(HttpClientNames.WebFunctionsClient)
            .AddTransientHttpErrorPolicy(GetRetryPolicy);

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

    private static AsyncRetryPolicy<HttpResponseMessage> GetRetryPolicy(
        PolicyBuilder<HttpResponseMessage> builder) =>
        builder.WaitAndRetryAsync(
            // See: https://brooker.co.za/blog/2015/03/21/backoff.html
            // Uses the "Jitter" algorithm
            Backoff.DecorrelatedJitterBackoffV2(
                medianFirstRetryDelay: TimeSpan.FromSeconds(1),
                retryCount: 5));
}
