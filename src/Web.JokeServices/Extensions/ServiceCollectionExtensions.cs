// Copyright (c) 2021 David Pine. All rights reserved.
//  Licensed under the MIT License.

using System;
using System.Net.Http;
using Learning.Blazor.JokeServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Retry;

namespace Learning.Blazor.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddJokeServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            if (services is null)
            {
                throw new ArgumentNullException(
                    nameof(services), "A service collection is required.");
            }

            static AsyncRetryPolicy<HttpResponseMessage> GetRetryPolicy(
                PolicyBuilder<HttpResponseMessage> builder) =>
                builder.WaitAndRetryAsync(
                    // See: https://brooker.co.za/blog/2015/03/21/backoff.html
                    // Uses the "Jitter" algorithm
                    Backoff.DecorrelatedJitterBackoffV2(
                            medianFirstRetryDelay: TimeSpan.FromSeconds(1),
                            retryCount: 5));

            services.AddScoped<IJokeService, ProgrammingJokeService>();
            services.AddScoped<IJokeService, DadJokeService>();
            services.AddScoped<IJokeService, ChuckNorrisJokeService>();

            services.AddHttpClient(nameof(DadJokeService),
                client => client.DefaultRequestHeaders.Add("Accept", "text/plain"))
                .AddTransientHttpErrorPolicy(GetRetryPolicy);

            services.AddHttpClient(nameof(ChuckNorrisJokeService),
                client => client.DefaultRequestHeaders.Add("Accept", "application/json"))
                .AddTransientHttpErrorPolicy(GetRetryPolicy);

            services.AddScoped<IJokeFactory, AggregateJokeFactory>();

            return services;
        }
    }
}
