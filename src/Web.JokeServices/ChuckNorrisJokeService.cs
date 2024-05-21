// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.JokeServices;

internal class ChuckNorrisJokeService(
    ILogger<ChuckNorrisJokeService> logger) : IJokeService
{
    private static readonly AsyncLazy<ChuckNorrisJoke[]?> s_embeddedJokes =
        new(async () =>
        {
            var @namespace = typeof(ChuckNorrisJokeService).Namespace;
            var resource = $"{@namespace}.Data.icndb-nerdy-jokes.json";

            var json = await ReadResourceFileAsync(resource);
            var jokes = json.FromJson<ChuckNorrisJoke[]>();

            return jokes;
        });

    JokeSourceDetails IJokeService.SourceDetails =>
        new(JokeSource.InternetChuckNorrisDatabase, new Uri("https://www.icndb.com/"));

    async Task<string?> IJokeService.GetJokeAsync()
    {
        try
        {
            var jokes = await s_embeddedJokes;
            if (jokes is { Length: > 0 })
            {
                var randomIndex = Random.Shared.Next(jokes.Length);
                var random = jokes[randomIndex];

                return random.Joke;
            }

            return null;
        }
        catch (Exception ex)
        {
            logger.LogError("Error getting something fun to say: {Error}", ex);
        }

        return null;
    }

    /// <summary>
    /// Read contents of an embedded resource file
    /// </summary>
    private static async Task<string> ReadResourceFileAsync(string fileName)
    {
        using var stream =
            Assembly.GetExecutingAssembly().GetManifestResourceStream(fileName);

        using var reader = new StreamReader(stream!);

        return await reader.ReadToEndAsync();
    }
}
