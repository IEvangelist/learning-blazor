// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Learning.Blazor.Api.Extensions;
using Learning.Blazor.Models;
using Microsoft.Extensions.Logging;

namespace Learning.Blazor.Api.Services
{
    public sealed class PwnedService
    {
        private readonly ILogger<PwnedService> _logger;
        private readonly HttpClient _apiClient;
        private readonly HttpClient _passwordsClient;

        public PwnedService(
            ILogger<PwnedService> logger,
            IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _apiClient = httpClientFactory.CreateClient(HttpClientNames.PwnedApiClient);
            _passwordsClient = httpClientFactory.CreateClient(HttpClientNames.PwnedPasswordsApiClient);
        }

        public async Task<IReadOnlySet<BreachHeader>> GetBreachesAsync(string emailAddress)
        {
            try
            {
                var breaches =
                        await _apiClient.GetFromJsonAsync<BreachHeader[]>(
                            $"breachedaccount/{emailAddress}");

                return (breaches ?? Array.Empty<BreachHeader>()).ToHashSet();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return Array.Empty<BreachHeader>().ToHashSet();
        }

        public async Task<BreachDetails?> GetBreachDetailsAsync(string breachName)
        {
            try
            {
                var breach =
                    await _apiClient.GetFromJsonAsync<BreachDetails>(
                        $"breach/{breachName}");

                return breach!;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return null!;
        }

        /// <summary>
        /// See: https://haveibeenpwned.com/api/v3#SearchingPwnedPasswordsByRange
        /// </summary>
        public async ValueTask<PwnedPassword> GetPwnedPasswordAsync(string? plainTextPassword)
        {
            PwnedPassword pwnedPassword = new(plainTextPassword);
            if (pwnedPassword.IsInvalid)
            {
                return pwnedPassword;
            }

            try
            {
                var passwordHash = plainTextPassword.ToSha1Hash()!;
                var firstFiveChars = passwordHash.Substring(0, 5);

                var passwordHashesInRange = await _passwordsClient.GetStringAsync($"range/{firstFiveChars}");
                if (passwordHashesInRange is not null)
                {
                    var hashCountMap =
                        passwordHashesInRange.Split("\r\n").Select(hashCountPair =>
                        {
                            var split = hashCountPair.Split(":");
                            return (Hash: split[0], Count: int.Parse(split[1]));
                        })
                        .ToDictionary(t => t.Hash, t => t.Count, StringComparer.OrdinalIgnoreCase);

                    var hashSuffix = passwordHash.Substring(5);
                    if (hashCountMap.TryGetValue(hashSuffix, out var count))
                    {
                        pwnedPassword = pwnedPassword with
                        {
                            HashedPassword = passwordHash,
                            PwnedCount = count,
                            IsPwned = count > 0,
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return pwnedPassword;
        }
    }
}
