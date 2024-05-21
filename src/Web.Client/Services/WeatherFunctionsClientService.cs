// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Services;

public sealed class WeatherFunctionsClientService(
    IHttpClientFactory httpClientFactory,
    IMemoryCache cache)
{
    public Task<WeatherDetails?> GetWeatherAsync(WeatherRequest request) =>
        cache.GetOrCreateAsync(
            request.Key,
            async options =>
            {
                options.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(9);

                var route = request.ToFormattedUrl();

                var client = httpClientFactory.CreateClient(HttpClientNames.WebFunctionsApi);

                var details =
                    await client.GetFromJsonAsync<WeatherDetails>(
                        $"/api/currentweather/{route}",
                        DefaultJsonSerialization.Options);

                return details is null ? null : details with { Units = request.Units };
            });
}
