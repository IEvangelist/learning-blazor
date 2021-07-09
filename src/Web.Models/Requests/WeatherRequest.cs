// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace Learning.Blazor.Models
{
    public record WeatherRequest
    {
        [JsonPropertyName("lang")] public string Language { get; init; } = null!;
        [JsonPropertyName("lat")] public decimal Latitude { get; init; }
        [JsonPropertyName("lon")] public decimal Longitude { get; init; }
        [JsonPropertyName("units")] public MeasurementSystem Units { get; init; }
        [JsonIgnore] public string Key => $"{Language}:{Latitude}:{Longitude}:{Units}";

        /// <summary>
        /// Returns the Azure Function URL for weather, after appling format value replacement.
        /// </summary>
        public string ToFormattedUrl(string weatherFunctionUrlFormat)
        {
            // Example: ".../api/currentweather/{lang}/{latitude}/{longitude}/{units}"

            return weatherFunctionUrlFormat
                .Replace("{lang}", Language)
                .Replace("{latitude}", Latitude.ToString())
                .Replace("{longitude}", Longitude.ToString())
                .Replace("{units}", Units.ToString());
        }
    }
}
