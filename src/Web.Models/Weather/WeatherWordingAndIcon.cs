// Copyright (c) 2021 David Pine. All rights reserved.
//  Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace Learning.Blazor.Models
{
    public record WeatherWordingAndIcon
    {
        [JsonPropertyName("main")] public string Main { get; init; } = null!;
        [JsonPropertyName("description")] public string Description { get; init; } = null!;
        [JsonPropertyName("icon")] public string Icon { get; init; } = null!;
    }
}
