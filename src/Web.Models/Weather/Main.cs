// Copyright (c) 2021 David Pine. All rights reserved.
//  Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace Learning.Blazor.Models
{
    public record Main
    {
        [JsonPropertyName("temp")]
        public double Temp { get; set; }

        [JsonPropertyName("temp_min")]
        public double TempMin { get; set; }

        [JsonPropertyName("temp_max")]
        public double TempMax { get; set; }

        [JsonPropertyName("pressure")]
        public double Pressure { get; set; }

        [JsonPropertyName("sea_level")]
        public double SeaLevel { get; set; }

        [JsonPropertyName("grnd_level")]
        public double GrndLevel { get; set; }

        [JsonPropertyName("humidity")]
        public int Humidity { get; set; }

        [JsonPropertyName("temp_kf")]
        public double TempKf { get; set; }
    }
}
