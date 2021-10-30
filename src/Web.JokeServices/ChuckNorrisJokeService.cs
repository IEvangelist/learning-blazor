﻿// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.JokeServices;

internal class ChuckNorrisJokeService : IJokeService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ChuckNorrisJokeService> _logger;

    public ChuckNorrisJokeService(
        IHttpClientFactory httpClientFactory,
        ILogger<ChuckNorrisJokeService> logger) =>
        (_httpClient, _logger) =
            (httpClientFactory.CreateClient(nameof(ChuckNorrisJokeService)), logger);

    JokeSourceDetails IJokeService.SourceDetails =>
        new(JokeSource.InternetChuckNorrisDatabase, new Uri("https://www.icndb.com/"));

    async Task<string?> IJokeService.GetJokeAsync()
    {
        try
        {
            var result = await _httpClient.GetFromJsonAsync<ChuckNorrisJoke>(
                "https://api.icndb.com/jokes/random?limitTo=[nerdy]", DefaultJsonSerialization.Options);

            return result?.Value?.Joke;
        }
        catch (Exception ex)
        {
            _logger.LogError("Error getting something fun to say: {Error}", ex);
        }

        return null;
    }
}
