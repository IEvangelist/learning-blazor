// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using Learning.Blazor.Http.Extensions;

namespace Learning.Blazor.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddJokeServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(nameof(services));

        services.AddScoped<IJokeService, ProgrammingJokeService>();
        services.AddScoped<IJokeService, DadJokeService>();
        services.AddScoped<IJokeService, ChuckNorrisJokeService>();

        services.AddHttpClient(nameof(DadJokeService),
            client => client.DefaultRequestHeaders.Add("Accept", "text/plain"))
            .AddDefaultTransientHttpErrorPolicy();

        services.AddHttpClient(nameof(ChuckNorrisJokeService),
            client => client.DefaultRequestHeaders.Add("Accept", "application/json"))
            .AddDefaultTransientHttpErrorPolicy();

        services.AddScoped<IJokeFactory, AggregateJokeFactory>();

        return services;
    }
}
