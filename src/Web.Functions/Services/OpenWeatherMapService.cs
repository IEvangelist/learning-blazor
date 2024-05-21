// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Functions.Services;

internal class OpenWeatherMapService(
    IHttpClientFactory httpClientFactory,
    IOptions<OpenWeatherMapOptions> options) : IWeatherService
{
    private readonly OpenWeatherMapOptions _openWeatherMapOptions = options.Value;

    public Task<WeatherDetails?> GetWeatherAsync(
        Coordinates coordinates, string? units, string? lang)
    {
        (var apiKey, var baseApiUrl) = _openWeatherMapOptions;
        (var lat, var lon) = coordinates;

        // Sensible defaults.
        units ??= "imperial";
        lang ??= "en";

        Dictionary<string, object> queryStringParameters = new()
        {
            ["lat"] = lat,
            ["lon"] = lon,
            ["appid"] = apiKey,
            ["units"] = units,
            ["lang"] = lang,
            ["exclude"] = "minutely"
        };

        var queryString =
            string.Join("&", queryStringParameters.Select(kvp => $"{kvp.Key}={kvp.Value}"));

        var requestUrl =
            $"{baseApiUrl}onecall?{queryString}";

        return httpClientFactory.CreateClient()
                .GetFromJsonAsync<WeatherDetails?>(
                    requestUrl, DefaultJsonSerialization.Options);
    }
}
