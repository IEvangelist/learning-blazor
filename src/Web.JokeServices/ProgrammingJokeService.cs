// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.JokeServices;

internal class ProgrammingJokeService : IJokeService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ProgrammingJokeService> _logger;

    public ProgrammingJokeService(
        HttpClient httpClient,
        ILogger<ProgrammingJokeService> logger) =>
        (_httpClient, _logger) = (httpClient, /* ArgumentNullException.ThrowIfNull(logger); */ logger);

    JokeSourceDetails IJokeService.SourceDetails =>
        new(JokeSource.OfficialJokeApiProgramming,
            new Uri("https://karljoke.herokuapp.com/"));

    async Task<string?> IJokeService.GetJokeAsync()
    {
        try
        {
            // An array with a single joke is returned
            var jokes = await _httpClient.GetFromJsonAsync<ProgrammingJoke[]>(
                "https://karljoke.herokuapp.com/jokes/programming/random",
                DefaultJsonSerialization.Options);

            return jokes?[0].Text;
        }
        catch (Exception ex)
        {
            _logger.LogError("Error getting something fun to say: {Error}", ex);
        }

        return null;
    }
}
