// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Api.Services;

public sealed class WeatherFunctionClientService : IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly IDistributedCache _cache;
    private readonly WebFunctionsOptions _functions;
    private readonly ILogger<WeatherFunctionClientService> _logger;

    public WeatherFunctionClientService(
        IHttpClientFactory httpClientFactory,
        IDistributedCache cache,
        IOptions<WebFunctionsOptions> options,
        ILogger<WeatherFunctionClientService> logger) =>
        (_httpClient, _cache, _functions, _logger) =
            (httpClientFactory.CreateClient(HttpClientNames.WebFunctionsClient), cache, options.Value, logger);

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

                return details! with { Units = request.Units };
            }, _logger);

    void IDisposable.Dispose() => _httpClient.Dispose();
}
