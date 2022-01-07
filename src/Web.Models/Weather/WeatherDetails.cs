// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace Learning.Blazor.Models;

public record class WeatherDetails
{
    [JsonPropertyName("lat")] public decimal Latitude { get; init; }
    [JsonPropertyName("lon")] public decimal Longitude { get; init; }
    [JsonPropertyName("timezone")] public string TimeZone { get; init; } = null!;
    [JsonPropertyName("timezone_offset")] public int TimeZoneOffset { get; init; }
    [JsonPropertyName("current")] public CurrentWeather Current { get; init; } = null!;
    [JsonPropertyName("hourly")] public IList<HourlyWeather> Hourly { get; init; } = Array.Empty<HourlyWeather>();
    [JsonPropertyName("daily")] public IList<DailyWeather> Daily { get; init; } = Array.Empty<DailyWeather>();
    [JsonPropertyName("alerts")] public IList<Alert> Alerts { get; init; } = Array.Empty<Alert>();
    [JsonPropertyName("units")] public int Units { get; init; }
}
