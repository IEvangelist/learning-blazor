// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.JokeServices;

internal class AggregateJokeFactory : IJokeFactory
{
    const string NotFunny = @"Did you hear the one about a joke service that failed to get jokes?
It's not very funny...";
    
    private readonly IList<IJokeService> _jokeServices;

    public AggregateJokeFactory(IEnumerable<IJokeService> jokeServices) =>
        _jokeServices = jokeServices.ToList();
    
    async Task<(string, JokeSourceDetails)> IJokeFactory.GetRandomJokeAsync()
    {
        string? joke = null;
        JokeSourceDetails sourceDetails = default;

        foreach (var service in _jokeServices.RandomOrder())
        {
            joke = await service.GetJokeAsync();
            sourceDetails = service.SourceDetails;

            if (joke is not null && sourceDetails != default)
            {
                break;
            }
        }

        return (
            joke ?? "There is nothing funny about this.",
            sourceDetails);
    }
}
