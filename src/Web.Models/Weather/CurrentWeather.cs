// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization;
using Learning.Blazor.Extensions;

namespace Learning.Blazor.Models;

public record class CurrentWeather : WeatherCommon
{
    [JsonPropertyName("temp")] public double Temperature { get; set; }
    [JsonPropertyName("feels_like")] public double FeelsLike { get; set; }
    [JsonPropertyName("sunrise")] public double Sunrise { get; set; }
    [JsonPropertyName("sunset")] public double Sunset { get; set; }

    [JsonIgnore]
    public DateTime SunriseDateTime => Sunrise.FromUnixTimeStamp();
    [JsonIgnore]
    public DateTime SunsetDateTime => Sunset.FromUnixTimeStamp();
}
