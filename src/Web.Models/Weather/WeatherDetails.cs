// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace Learning.Blazor.Models;

public record class WeatherDetails
{
    [JsonPropertyName("lat")] public decimal Latitude { get; set; }
    [JsonPropertyName("lon")] public decimal Longitude { get; set; }
    [JsonPropertyName("timezone")] public string TimeZone { get; set; } = null!;
    [JsonPropertyName("timezone_offset")] public int TimeZoneOffset { get; set; }
    [JsonPropertyName("current")] public CurrentWeather Current { get; set; } = null!;
    [JsonPropertyName("hourly")] public IList<HourlyWeather> Hourly { get; set; } = Array.Empty<HourlyWeather>();
    [JsonPropertyName("daily")] public IList<DailyWeather> Daily { get; set; } = Array.Empty<DailyWeather>();
    [JsonPropertyName("alerts")] public IList<Alert> Alerts { get; set; } = Array.Empty<Alert>();
    [JsonPropertyName("units")] public int Units { get; set; }
}
