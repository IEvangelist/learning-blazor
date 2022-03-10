// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace Learning.Blazor.Models;

public record class WeatherCommon : WeatherDate
{
    [JsonPropertyName("pressure")]
    public double Pressure { get; set; }

    [JsonPropertyName("humidity")]
    public double Humidity { get; set; }

    [JsonPropertyName("dew_point")]
    public double DewPoint { get; set; }

    [JsonPropertyName("uvi")]
    public double UltravioletIndex { get; set; }

    [JsonPropertyName("clouds")]
    public int Clouds { get; set; }

    [JsonPropertyName("visibility")]
    public int Visibility { get; set; }

    [JsonPropertyName("wind_speed")]
    public double WindSpeed { get; set; }

    [JsonPropertyName("wind_deg")]
    public int WindDegree { get; set; }

    [JsonPropertyName("weather")]
    public IList<WeatherWordingAndIcon> Weather { get; set; } = Array.Empty<WeatherWordingAndIcon>();
}
