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
        var count = _jokeServices.Count;
        var visited = new HashSet<int>(count);

        return await _jokeServices.UseRandomAsync(
            () => visited.Count < count,
            visited.Add,
            async service =>
            {
                var joke = await service.GetJokeAsync();
                var sourceDetails = service.SourceDetails;

                var result = (
                    joke ?? NotFunny,
                    sourceDetails);

                return (joke is not null, result);
            },
            (NotFunny, default));
    }
}
