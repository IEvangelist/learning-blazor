// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization;
using Learning.Blazor.Extensions;

namespace Learning.Blazor.Models
{
    public record CurrentWeather : WeatherCommon
    {
        [JsonPropertyName("temp")] public double Temperature { get; init; }
        [JsonPropertyName("feels_like")] public double FeelsLike { get; init; }
        [JsonPropertyName("sunrise")] public double Sunrise { get; init; }
        [JsonPropertyName("sunset")] public double Sunset { get; init; }

        [JsonIgnore]
        public DateTime SunriseDateTime => Sunrise.FromUnixTimeStamp();
        [JsonIgnore]
        public DateTime SunsetDateTime => Sunset.FromUnixTimeStamp();
    }
}
