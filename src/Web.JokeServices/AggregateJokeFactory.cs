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
        var randomJokeService = _jokeServices.RandomElement();

        var joke =
            await randomJokeService.GetJokeAsync()
            ?? "There is nothing funny about this.";

        return (joke, randomJokeService.SourceDetails);
    }
}
