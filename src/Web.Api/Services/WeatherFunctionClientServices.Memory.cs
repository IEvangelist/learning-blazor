// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Api.Services;

public sealed class WeatherFunctionClientService : IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly IMemoryCache _cache;
    private readonly WebFunctionsOptions _functions;

    public WeatherFunctionClientService(
        IHttpClientFactory httpClientFactory,
        IMemoryCache cache,
        IOptions<WebFunctionsOptions> options) =>
        (_httpClient, _cache, _functions) =
            (httpClientFactory.CreateClient(HttpClientNames.WebFunctionsClient), cache, options.Value);

    public Task<WeatherDetails?> GetWeatherAsync(WeatherRequest request) =>
        _cache.GetOrCreateAsync(
            request.Key,
            async options =>
            {
                options.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(9);

                var requestUrl = request.ToFormattedUrl(_functions.WeatherFunctionUrlFormat);
                var details =
                    await _httpClient.GetFromJsonAsync<WeatherDetails>(
                        requestUrl, DefaultJsonSerialization.Options);

                return details is null ? null : details with { Units = request.Units };
            });

    void IDisposable.Dispose() => _httpClient.Dispose();
}
