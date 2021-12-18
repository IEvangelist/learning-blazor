// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTwitterServices(
        this IServiceCollection services,
        IConfiguration configuration) =>
        services.AddSingleton<ITwitterService, DefaultTwitterService>()
            .AddSingleton(services =>
            {

                TwitterClient client = new(
                    configuration["Authentication:Twitter:ConsumerKey"],
                    configuration["Authentication:Twitter:ConsumerSecret"],
                    configuration["Authentication:Twitter:AccessToken"],
                    configuration["Authentication:Twitter:AccessTokenSecret"]);

                client.Events.OnTwitterException += (_, ex) =>
                {
                    var logger =
                        services.GetRequiredService<ILogger<DefaultTwitterService>>();

                    logger.LogError("Tweetinvi Exception: {Ex}", ex);
                };

                return client;
            })
            .AddSingleton(services =>
            {
                var client = services.GetRequiredService<TwitterClient>();
                return client.Streams.CreateFilteredStream()!;
            });
}
