// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace Learning.Blazor.Models
{
    public record GeoCodeRequest
    {
        [JsonPropertyName("lang")] public string Language { get; init; } = null!;
        [JsonPropertyName("lat")] public decimal Latitude { get; init; }
        [JsonPropertyName("lon")] public decimal Longitude { get; init; }
        [JsonIgnore] public string Key => $"GCR:{Language}:{Latitude}:{Longitude}";

        public void Deconstruct(
            out decimal latitude,
            out decimal longitude,
            out string language) =>
            (latitude, longitude, language) = (Latitude, Longitude, Language);
    }
}
