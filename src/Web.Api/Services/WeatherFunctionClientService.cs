// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.Options;
using Microsoft.Extensions.Caching.Memory;
using Learning.Blazor.Extensions;
using Learning.Blazor.Models;
using Learning.Blazor.Api.Extensions;
using Learning.Blazor.Api.Options;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Json;

namespace Learning.Blazor.Api.Services
{
    public sealed class WeatherFunctionClientService : IAsyncDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly IDistributedCache _cache;
        private readonly WebFunctionsOptions _functions;

        public WeatherFunctionClientService(
            IHttpClientFactory httpClientFactory,
            IDistributedCache cache,
            IOptions<WebFunctionsOptions> options) =>
            (_httpClient, _cache, _functions) =
                (httpClientFactory.CreateClient("Web.Functions"), cache, options.Value);

        public Task<WeatherDetails> GetWeatherAsync(WeatherRequest request) =>
            _cache.GetOrCreateAsync(
                request.Key,
                async () =>
                {
                    var requestUrl = request.ToFormattedUrl(_functions.WeatherFunctionUrlFormat);
                    var details =
                        await _httpClient.GetFromJsonAsync<WeatherDetails>(
                            requestUrl, DefaultJsonSerialization.Options);

                    // We need to ensure that the weather details model and return their measurement system.
                    if (details is not null)
                    {
                        details.MeasurementSystem = request.Units;
                    }

                    return details!;
                });

        ValueTask IAsyncDisposable.DisposeAsync() =>
            _httpClient.TryDisposeAsync();
    }
}
