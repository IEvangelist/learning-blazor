// Copyright (c) 2021 David Pine. All rights reserved.
//  Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace Learning.Blazor.Models
{
    public record FeelsLike
    {
        [JsonPropertyName("day")] public double Day { get; init; }
        [JsonPropertyName("night")] public double Night { get; init; }
        [JsonPropertyName("eve")] public double Evening { get; init; }
        [JsonPropertyName("morn")] public double Morning { get; init; }
    }
}
