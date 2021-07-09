// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Text.Json.Serialization;
using Learning.Blazor.Extensions;

namespace Learning.Blazor.Models
{
    public record Alert
    {
        [JsonPropertyName("event")] public string Event { get; init; } = null!;
        [JsonPropertyName("start")] public double Start { get; init; }
        [JsonPropertyName("end")] public double End { get; init; }
        [JsonPropertyName("description")] public string Description { get; init; } = null!;

        public DateTime StartDate => Start.FromUnixTimeStamp();
        public DateTime EndDate => End.FromUnixTimeStamp();
    }
}
