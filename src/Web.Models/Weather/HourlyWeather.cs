// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace Learning.Blazor.Models;

public record class HourlyWeather : WeatherCommon
{
    [JsonPropertyName("wind_gust")] public double WindGust { get; set; }

    [JsonPropertyName("temp")] public double Temperature { get; set; }

    [JsonPropertyName("feels_like")] public double FeelsLike { get; set; }
}
