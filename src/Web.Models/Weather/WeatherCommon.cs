// Copyright (c) 2021 David Pine. All rights reserved.
//  Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Learning.Blazor.Models
{
    public record WeatherCommon : WeatherDate
    {
        [JsonPropertyName("pressure")]
        public double Pressure { get; init; }

        [JsonPropertyName("humidity")]
        public double Humidity { get; init; }

        [JsonPropertyName("dew_point")]
        public double DewPoint { get; init; }

        [JsonPropertyName("uvi")]
        public double UltravioletIndex { get; init; }

        [JsonPropertyName("clouds")]
        public int Clouds { get; init; }

        [JsonPropertyName("visibility")]
        public int Visibility { get; init; }

        [JsonPropertyName("wind_speed")]
        public double WindSpeed { get; init; }

        [JsonPropertyName("wind_deg")]
        public int WindDegree { get; init; }

        [JsonPropertyName("weather")]
        public IList<WeatherWordingAndIcon> Weather { get; init; } = Array.Empty<WeatherWordingAndIcon>();
    }
}
