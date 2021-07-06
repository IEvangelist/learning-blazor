// Copyright (c) 2021 David Pine. All rights reserved.
//  Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace Learning.Blazor.Models
{
    public record HourlyWeather : WeatherCommon
    {
        [JsonPropertyName("wind_gust")] public double WindGust { get; init; }

        [JsonPropertyName("temp")] public double Temperature { get; init; }

        [JsonPropertyName("feels_like")] public double FeelsLike { get; init; }
    }
}
