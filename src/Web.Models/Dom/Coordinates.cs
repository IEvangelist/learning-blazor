// Copyright (c) 2021 David Pine. All rights reserved.
//  Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace Learning.Blazor.Models
{
    public record Coordinates(
        [property: JsonPropertyName("lat")] decimal Latitude,
        [property: JsonPropertyName("lon")] decimal Longitude);
}
