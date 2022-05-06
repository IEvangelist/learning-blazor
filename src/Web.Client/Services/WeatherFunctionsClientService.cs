// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Services;

public sealed class WeatherFunctionsClientService : IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly IMemoryCache _cache;

    public WeatherFunctionsClientService(
        IHttpClientFactory httpClientFactory,
        IMemoryCache cache) =>
        (_httpClient, _cache) =
            (httpClientFactory.CreateClient(HttpClientNames.WebFunctionsApi), cache);

    public Task<WeatherDetails?> GetWeatherAsync(WeatherRequest request) =>
        _cache.GetOrCreateAsync(
            request.Key,
            async options =>
            {
                options.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(9);

                var route = request.ToFormattedUrl();
                var details =
                    await _httpClient.GetFromJsonAsync<WeatherDetails>(
                        $"/api/currentweather/{route}",
                        DefaultJsonSerialization.Options);

                return details is null ? null : details with { Units = request.Units };
            });

    void IDisposable.Dispose() => _httpClient.Dispose();
}
