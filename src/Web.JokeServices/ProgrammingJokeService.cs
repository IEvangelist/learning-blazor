// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Learning.Blazor.Models;
using Microsoft.Extensions.Logging;

namespace Learning.Blazor.JokeServices
{
    internal class ProgrammingJokeService : IJokeService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ProgrammingJokeService> _logger;
        private readonly JsonSerializerOptions _options = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public ProgrammingJokeService(
            HttpClient httpClient,
            ILogger<ProgrammingJokeService> logger) =>
            (_httpClient, _logger) = (httpClient, logger);

        JokeSourceDetails IJokeService.SourceDetails =>
            new(JokeSource.OfficialJokeApiProgramming,
                new Uri("https://official-joke-api.appspot.com/"));

        async Task<string?> IJokeService.GetJokeAsync()
        {
            try
            {
                // An array with a single joke is returned
                var jokes = await _httpClient.GetFromJsonAsync<ProgrammingJoke[]>(
                    "https://official-joke-api.appspot.com/jokes/programming/random", _options);

                return jokes?[0].Text;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error getting something fun to say: {Error}", ex);
            }

            return null;
        }
    }
}
