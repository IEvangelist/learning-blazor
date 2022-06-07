// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System.Reflection;
using Blazor.Serialization.Extensions;

namespace Learning.Blazor.JokeServices;

internal class ChuckNorrisJokeService : IJokeService
{
    private readonly ILogger<ChuckNorrisJokeService> _logger;
    private static readonly AsyncLazy<ChuckNorrisJoke[]?> s_embeddedJokes =
        new(async () =>
        {
            var nmspc = typeof(ChuckNorrisJokeService).Namespace;
            var resource = $"{nmspc}.Data.icndb-nerdy-jokes.json";

            var json = await ReadResourceFileAsync(resource);
            var jokes = json.FromJson<ChuckNorrisJoke[]>();

            return jokes;
        });

    public ChuckNorrisJokeService(
        ILogger<ChuckNorrisJokeService> logger) => _logger = logger;

    JokeSourceDetails IJokeService.SourceDetails =>
        new(JokeSource.InternetChuckNorrisDatabase, new Uri("https://www.icndb.com/"));

    async Task<string?> IJokeService.GetJokeAsync()
    {
        try
        {
            var jokes = await s_embeddedJokes;
            if (jokes is { Length: > 0 })
            {
                var randomIndex = Random.Shared.Next(0, jokes.Length);
                var joke = jokes[randomIndex];

                return joke.Joke;
            }            

            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError("Error getting something fun to say: {Error}", ex);
        }

        return null;
    }

    /// <summary>
    /// Read contents of an embedded resource file
    /// </summary>
    private static async Task<string> ReadResourceFileAsync(string fileName)
    {
        var thisAssembly = Assembly.GetExecutingAssembly();
        using (var stream = thisAssembly.GetManifestResourceStream(fileName))
        {
            using (var reader = new StreamReader(stream!))
            {
                return await reader.ReadToEndAsync();
            }
        }
    }
}
