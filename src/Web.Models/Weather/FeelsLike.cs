// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace Learning.Blazor.Models;

public record class FeelsLike
{
    [JsonPropertyName("day")] public double Day { get; set; }
    [JsonPropertyName("night")] public double Night { get; set; }
    [JsonPropertyName("eve")] public double Evening { get; set; }
    [JsonPropertyName("morn")] public double Morning { get; set; }
}
