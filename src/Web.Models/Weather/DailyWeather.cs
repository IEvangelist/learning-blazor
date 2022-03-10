// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization;
using Learning.Blazor.Extensions;

namespace Learning.Blazor.Models
{
    public record class DailyWeather : WeatherCommon
    {
        [JsonPropertyName("sunrise")]
        public double Sunrise { get; set; }

        [JsonIgnore]
        public DateTime SunriseDateTime => Sunrise.FromUnixTimeStamp();

        [JsonPropertyName("sunset")]
        public double Sunset { get; set; }

        [JsonIgnore]
        public DateTime SunsetDateTime => Sunset.FromUnixTimeStamp();

        [JsonPropertyName("moonrise")]
        public double Moonrise { get; set; }

        [JsonIgnore]
        public DateTime MoonriseDateTime => Moonrise.FromUnixTimeStamp();

        [JsonPropertyName("moonset")]
        public double Moonset { get; set; }

        [JsonIgnore]
        public DateTime MoonsetDateTime => Moonset.FromUnixTimeStamp();

        /// <summary>
        /// Moon phase. 0 and 1 are 'new moon', 0.25 is
        /// 'first quarter moon', 0.5 is 'full moon' and 0.75 is
        /// 'last quarter moon'. The periods in between are
        /// called 'waxing crescent', 'waxing gibous', 'waning gibous',
        /// and 'waning crescent', respectively.
        /// </summary>
        [JsonPropertyName("moon_phase")]
        public double MoonPhase { get; set; }

        [JsonPropertyName("temp")] public Temperature Temperature { get; set; } = null!;
        [JsonPropertyName("feels_like")] public FeelsLike FeelsLike { get; set; } = null!;
    }
}
