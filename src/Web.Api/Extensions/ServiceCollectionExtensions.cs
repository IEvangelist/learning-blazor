// Copyright (c) 2021 David Pine. All rights reserved.
//  Licensed under the MIT License.

using System;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Contrib.WaitAndRetry;
using Learning.Blazor.Extensions;
using Learning.Blazor.Api.Services;

namespace Learning.Blazor.Api.Extensions
{
    internal static class ServiceCollectionExtensions
    {
        internal static IServiceCollection AddApiServices(
            this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient("Web.Functions")
                .AddTransientHttpErrorPolicy(_ =>
                    (IAsyncPolicy<HttpResponseMessage>)
                        Policy.Handle<Exception>()
                            .WaitAndRetryAsync(
                                // See: https://brooker.co.za/blog/2015/03/21/backoff.html
                                // Uses the "Jitter" algorithm
                                Backoff.DecorrelatedJitterBackoffV2(
                                    medianFirstRetryDelay: TimeSpan.FromSeconds(1),
                                    retryCount: 5)));

            services.AddScoped<WeatherFunctionClientService>();

            services.AddJokeServices(configuration);

            return services;
        }
    }
}
