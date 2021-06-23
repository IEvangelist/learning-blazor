// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Learning.Blazor.Models;

namespace Learning.Blazor.Services
{
    public class ProgrammingJokeService : IJokeService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ProgrammingJokeService> _logger;

        JsonSerializerOptions _options = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public ProgrammingJokeService(
            HttpClient httpClient,
            ILogger<ProgrammingJokeService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<Joke?> GetJokeAsync()
        {
            try
            {
                // An array with a single joke is returned
                Joke[]? jokes = await _httpClient.GetFromJsonAsync<ProgrammingJoke[]>(
                    "https://official-joke-api.appspot.com/jokes/programming/random", _options);

                return jokes?[0];
            }
            catch (Exception ex)
            {
                _logger.LogError("Error getting something fun to say: {Error}", ex);
            }

            return null;
        }
    }
}
