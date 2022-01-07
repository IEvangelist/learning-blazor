// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace Learning.Blazor.Models;

public record class Position
{
    [JsonPropertyName("coords")]
    public Coordinates Coordinates { get; init; } = null!;
}
