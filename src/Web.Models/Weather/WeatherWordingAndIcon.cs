// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace Learning.Blazor.Models;

public record class WeatherWordingAndIcon
{
    [JsonPropertyName("main")] public string Main { get; set; } = null!;
    [JsonPropertyName("description")] public string Description { get; set; } = null!;
    [JsonPropertyName("icon")] public string Icon { get; set; } = null!;
}
