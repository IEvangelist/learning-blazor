// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Retry;

namespace Learning.Blazor.Http.Extensions;

public static class HttpClientBuilderRetryPolicyExtensions
{
    /// <summary>
    /// Adds the default transient HTTP error policy to
    /// the given <paramref name="builder"/> instance.
    /// </summary>
    /// <param name="builder">The HTTP client builder instance to add to.</param>
    /// <returns>The same HTTP client builder instance with the added policy.</returns>
    public static IHttpClientBuilder AddDefaultTransientHttpErrorPolicy(
        this IHttpClientBuilder builder) =>
        builder.AddTransientHttpErrorPolicy(GetDefaultRetryPolicy);

    /// <summary>
    /// Gets the default retry policy, using the "jitter" algorithm
    /// which will wait and retry.
    /// </summary>
    /// <param name="builder">The policy builder instance to apply the backoff to.</param>
    /// <returns>An async retry policy configured with the default retry policy.</returns>
    public static AsyncRetryPolicy<HttpResponseMessage> GetDefaultRetryPolicy(
        PolicyBuilder<HttpResponseMessage> builder) =>
        builder.WaitAndRetryAsync(
            // See: https://brooker.co.za/blog/2015/03/21/backoff.html
            // Uses the "Jitter" algorithm
            Backoff.DecorrelatedJitterBackoffV2(
                medianFirstRetryDelay: TimeSpan.FromSeconds(1),
                retryCount: 5));
}

