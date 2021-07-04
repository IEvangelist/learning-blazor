// Copyright (c) 2021 David Pine. All rights reserved.
//  Licensed under the MIT License.

using System.Net.Http;
using System.Threading.Tasks;
using Learning.Blazor.Models;
using Microsoft.Extensions.Options;
using Web.Weather.Options;

namespace Web.Weather.Services
{
    internal class OpenWeatherMapService : IWeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly OpenWeatherMapOptions _openWeatherMapOptions;

        public OpenWeatherMapService(
            HttpClient httpClient,
            IOptions<OpenWeatherMapOptions> options) =>
            (_httpClient, _openWeatherMapOptions) = (httpClient, options.Value);

        public Task<CurrentWeather> GetCurrentWeatherAsync(
            Coordinates coordinates, string units)
        {
            var (apiKey, baseApiUrl)= _openWeatherMapOptions;
            var (lat, lon) = coordinates;
            var requestUrl =
                $"{baseApiUrl}weather?lat={lat}&lon={lon}&appid={apiKey}&units={units}";

            return Task.FromResult(null as CurrentWeather);
        }

        public Task<ForecastWeather> GetForecastWeatherAsync(
            Coordinates coordinates, string units)
        {
            var (apiKey, baseApiUrl) = _openWeatherMapOptions;
            var (lat, lon) = coordinates;
            var requestUrl =
                $"{baseApiUrl}onecall?lat={lat}&lon={lon}&appid={apiKey}&units={units}&exclude=current,minutely,hourly";

            return Task.FromResult(null as ForecastWeather);
        }
    }
}
