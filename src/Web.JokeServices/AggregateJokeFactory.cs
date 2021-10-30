// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.JokeServices;

internal class AggregateJokeFactory : IJokeFactory
{
    private readonly IEnumerable<IJokeService> _jokeServices;

    public AggregateJokeFactory(IEnumerable<IJokeService> jokeServices) =>
        _jokeServices = jokeServices;

    async Task<(string, JokeSourceDetails)> IJokeFactory.GetRandomJokeAsync()
    {
        var services = _jokeServices;
        var randomService = services.RandomElement();

        var joke = await randomService.GetJokeAsync();
        var sourceDetails = randomService.SourceDetails;

        while (joke is null && services.Any())
        {
            services = services.Except(new[] { randomService });
            randomService = services.RandomElement();

            joke = await randomService.GetJokeAsync();
            sourceDetails = randomService.SourceDetails;
        }

        return (
            joke ?? "There is nothing funny about this.",
            sourceDetails);
    }
}
