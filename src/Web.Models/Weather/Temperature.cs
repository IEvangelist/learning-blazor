// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace Learning.Blazor.Models;

public record class Temperature : FeelsLike
{
    [JsonPropertyName("min")] public double Min { get; set; }

    [JsonPropertyName("max")] public double Max { get; set; }
}
