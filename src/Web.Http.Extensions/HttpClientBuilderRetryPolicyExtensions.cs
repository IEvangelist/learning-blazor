// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Retry;

namespace Learning.Blazor.Http.Extensions;

public static class HttpClientBuilderRetryPolicyExtensions
{
    public static IHttpClientBuilder AddDefaultTransientHttpErrorPolicy(
        this IHttpClientBuilder builder) =>
        builder.AddTransientHttpErrorPolicy(GetDefaultRetryPolicy);

    public static AsyncRetryPolicy<HttpResponseMessage> GetDefaultRetryPolicy(
        PolicyBuilder<HttpResponseMessage> builder) =>
        builder.WaitAndRetryAsync(
            // See: https://brooker.co.za/blog/2015/03/21/backoff.html
            // Uses the "Jitter" algorithm
            Backoff.DecorrelatedJitterBackoffV2(
                medianFirstRetryDelay: TimeSpan.FromSeconds(1),
                retryCount: 5));
}

