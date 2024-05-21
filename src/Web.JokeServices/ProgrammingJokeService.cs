// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.JokeServices;

internal class ProgrammingJokeService(
    HttpClient httpClient,
    ILogger<ProgrammingJokeService> logger) : IJokeService
{
    JokeSourceDetails IJokeService.SourceDetails =>
        new(JokeSource.RandomProgrammingJokeApi,
            new Uri("https://karljoke.herokuapp.com/"));

    async Task<string?> IJokeService.GetJokeAsync()
    {
        try
        {
            httpClient.DefaultRequestHeaders.Accept.ParseAdd(
                MediaTypeNames.Application.Json);

            // An array with a single joke is returned
            var jokes = await httpClient.GetFromJsonAsync<ProgrammingJoke[]>(
                "https://karljoke.herokuapp.com/jokes/programming/random",
                DefaultJsonSerialization.Options);

            return jokes?[0].Text;
        }
        catch (Exception ex)
        {
            logger.LogError("Error getting something fun to say: {Error}", ex);
        }

        return null;
    }
}
