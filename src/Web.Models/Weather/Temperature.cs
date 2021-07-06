// Copyright (c) 2021 David Pine. All rights reserved.
//  Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace Learning.Blazor.Models
{
    public record Temperature : FeelsLike
    {
        [JsonPropertyName("min")] public double Min { get; init; }

        [JsonPropertyName("max")] public double Max { get; init; }
    }
}
