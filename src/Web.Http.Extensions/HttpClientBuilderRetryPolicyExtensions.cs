// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http.Resilience;

namespace Learning.Blazor.Http.Extensions;

public static class HttpClientBuilderRetryPolicyExtensions
{
    /// <summary>
    /// Adds the default transient HTTP error policy to
    /// the given <paramref name="builder"/> instance.
    /// </summary>
    /// <param name="builder">The HTTP client builder instance to add to.</param>
    /// <returns>The same HTTP client builder instance with the added policy.</returns>
    public static IHttpStandardResiliencePipelineBuilder AddDefaultTransientHttpErrorPolicy(
        this IHttpClientBuilder builder) =>
        builder.AddStandardResilienceHandler();

    /// <summary>
    /// Gets the default retry policy, using the "jitter" algorithm
    /// which will wait and retry.
    /// </summary>
    /// <param name="builder">The policy builder instance to apply the backoff to.</param>
    /// <returns>An async retry policy configured with the default retry policy.</returns>
    public static HttpStandardResilienceOptions GetDefaultRetryPolicy() =>
        new()
        {
        };
}

