// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization;
using Learning.Blazor.Extensions;

namespace Learning.Blazor.Models;

public sealed record class Alert
{
    [JsonPropertyName("sender_name")] public string SenderName { get; set; } = null!;
    [JsonPropertyName("event")] public string Event { get; set; } = null!;
    [JsonPropertyName("start")] public double Start { get; set; }
    [JsonPropertyName("end")] public double End { get; set; }
    [JsonPropertyName("description")] public string Description { get; set; } = null!;
    [JsonPropertyName("tag")] public string[] Tags { get; set; } = [];

    [JsonIgnore]
    public DateTime StartDate => Start.FromUnixTimeStamp();

    [JsonIgnore]
    public DateTime EndDate => End.FromUnixTimeStamp();
}
