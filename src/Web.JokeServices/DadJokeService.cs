// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.JokeServices;

internal sealed class DadJokeService(
    HttpClient httpClient,
    ILogger<DadJokeService> logger) : IJokeService
{
    JokeSourceDetails IJokeService.SourceDetails =>
        new(JokeSource.ICanHazDadJoke, new Uri("https://icanhazdadjoke.com/"));

    async Task<string?> IJokeService.GetJokeAsync()
    {
        try
        {
            httpClient.DefaultRequestHeaders.Accept.ParseAdd(
                MediaTypeNames.Application.Json);

            var result = await httpClient.GetFromJsonAsync<DadJoke>(
                "https://icanhazdadjoke.com/", DefaultJsonSerialization.Options);

            return result?.Joke;
        }
        catch (Exception ex)
        {
            logger.LogError("Error getting something fun to say: {Error}", ex);
        }

        return null;
    }
}
