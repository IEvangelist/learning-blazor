// Copyright (c) 2021 David Pine. All rights reserved.
//  Licensed under the MIT License.

using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Caching.Memory;
using Learning.Blazor.Models;
using Learning.Blazor.Api.Options;
using Learning.Blazor.Api.Requests;

namespace Learning.Blazor.Api.Services
{
    internal class WeatherFunctionClientService : IAsyncDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _memoryCache;
        private readonly WebFunctionsOptions _functions;

        public WeatherFunctionClientService(
            IHttpClientFactory httpClientFactory,
            IMemoryCache memoryCache,
            IOptions<WebFunctionsOptions> options) =>
            (_httpClient, _memoryCache, _functions) =
                (httpClientFactory.CreateClient("Web.Functions"), memoryCache, options.Value);

        public Task<WeatherDetails?> GetWeatherAsync(WeatherRequest request) =>
            _memoryCache.GetOrCreateAsync(
                request,
                async entry =>
                {
                    // It's weather data, it doesn't change that often.
                    // Let's cache it for five mins, and slide out 1.
                    entry.SetSlidingExpiration(TimeSpan.FromMinutes(1));
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);

                    string formattedUrl = request.ToFormattedUrl(_functions.WeatherFunctionUrlFormat);
                    return await _httpClient.GetFromJsonAsync<WeatherDetails?>(formattedUrl);
                });

        public async ValueTask DisposeAsync()
        {
            if (_httpClient is IAsyncDisposable asyncDisposable)
            {
                await asyncDisposable.ConfigureAwait(false).DisposeAsync();
            }
            else if (_httpClient is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }
}
