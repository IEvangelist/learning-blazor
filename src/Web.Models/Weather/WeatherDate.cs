// Copyright (c) 2021 David Pine. All rights reserved.
//  Licensed under the MIT License.

using System;
using System.Text.Json.Serialization;
using Learning.Blazor.Extensions;

namespace Learning.Blazor.Models
{
    public record WeatherDate
    {
        [JsonPropertyName("dt")] public double Dt { get; init; }

        public DateTime DateTime => Dt.FromUnixTimeStamp();
    }
}
