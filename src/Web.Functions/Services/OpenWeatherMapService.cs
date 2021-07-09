// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Learning.Blazor.Functions.Options;
using Learning.Blazor.Models;
using Microsoft.Extensions.Options;

namespace Learning.Blazor.Functions.Services
{
    internal class OpenWeatherMapService : IWeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly OpenWeatherMapOptions _openWeatherMapOptions;

        public OpenWeatherMapService(
            HttpClient httpClient,
            IOptions<OpenWeatherMapOptions> options) =>
            (_httpClient, _openWeatherMapOptions) = (httpClient, options.Value);

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

            return _httpClient.GetFromJsonAsync<WeatherDetails?>(requestUrl);
        }
    }
}
