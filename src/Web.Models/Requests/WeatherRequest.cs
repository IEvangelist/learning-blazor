// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System.Globalization;
using System.Text.Json.Serialization;

namespace Learning.Blazor.Models;

public record class WeatherRequest
{
    [JsonPropertyName("lang")] public string Language { get; set; } = null!;
    [JsonPropertyName("lat")] public decimal Latitude { get; set; }
    [JsonPropertyName("lon")] public decimal Longitude { get; set; }
    [JsonPropertyName("units")] public int Units { get; set; }
    [JsonIgnore] public string Key => $"WR:{Language}:{Latitude}:{Longitude}:{Units}";

    /// <summary>
    /// Returns the Azure Function URL for weather, after appling format value replacement.
    /// </summary>
    public string ToFormattedUrl() =>
        string.Create(
            CultureInfo.GetCultureInfo("en-US"),
            $"{Language}/{Latitude}/{Longitude}/{(MeasurementSystem)Units}");
}
