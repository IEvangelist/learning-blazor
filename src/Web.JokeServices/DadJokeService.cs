// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.JokeServices;

internal class DadJokeService : IJokeService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<DadJokeService> _logger;

    public DadJokeService(
        HttpClient httpClient,
        ILogger<DadJokeService> logger) =>
        (_httpClient, _logger) = (httpClient, logger);

    JokeSourceDetails IJokeService.SourceDetails =>
        new(JokeSource.ICanHazDadJoke, new Uri("https://icanhazdadjoke.com/"));

    async Task<string?> IJokeService.GetJokeAsync()
    {
        try
        {
            _httpClient.DefaultRequestHeaders.Accept.ParseAdd(
                MediaTypeNames.Application.Json);

            var result = await _httpClient.GetFromJsonAsync<DadJoke>(
                "https://icanhazdadjoke.com/", DefaultJsonSerialization.Options);

            return result?.Joke;
        }
        catch (Exception ex)
        {
            _logger.LogError("Error getting something fun to say: {Error}", ex);
        }

        return null;
    }
}
