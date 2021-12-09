// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using Learning.Blazor.Http.Extensions;

namespace Learning.Blazor.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddJokeServices(
        this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(nameof(services));

        services.AddScoped<IJokeService, ProgrammingJokeService>();
        services.AddScoped<IJokeService, DadJokeService>();
        services.AddScoped<IJokeService, ChuckNorrisJokeService>();            

        services.AddHttpClient<ProgrammingJokeService>()
            .AddDefaultTransientHttpErrorPolicy();
        services.AddHttpClient<DadJokeService>()
            .AddDefaultTransientHttpErrorPolicy();
        services.AddHttpClient<ChuckNorrisJokeService>()
            .AddDefaultTransientHttpErrorPolicy();

        services.AddScoped<IJokeFactory, AggregateJokeFactory>();

        return services;
    }
}
