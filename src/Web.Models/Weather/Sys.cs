// Copyright (c) 2021 David Pine. All rights reserved.
//  Licensed under the MIT License.

using System;
using System.Text.Json.Serialization;
using Learning.Blazor.Extensions;

namespace Learning.Blazor.Models
{
    public record Sys
    {
        [JsonPropertyName("type")]
        public int Type { get; set; }

        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("message")]
        public double Message { get; set; }

        [JsonPropertyName("country")]
        public string Country { get; set; } = null!;

        [JsonPropertyName("sunrise")]
        public double Sunrise { get; set; }

        public DateTime SunriseDateTime => Sunrise.FromUnixTimeStamp();

        [JsonPropertyName("sunset")]
        public double Sunset { get; set; }

        public DateTime SunsetDateTime => Sunset.FromUnixTimeStamp();

        [JsonPropertyName("population")]
        public int Population { get; set; }
    }
}
