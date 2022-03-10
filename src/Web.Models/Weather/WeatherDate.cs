// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization;
using Learning.Blazor.Extensions;

namespace Learning.Blazor.Models;

public record class WeatherDate
{
    [JsonPropertyName("dt")] public double Dt { get; set; }

    [JsonIgnore]
    public DateTime DateTime => Dt.FromUnixTimeStamp();
}
