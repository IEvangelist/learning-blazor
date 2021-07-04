// Copyright (c) 2021 David Pine. All rights reserved.
//  Licensed under the MIT License.

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Learning.Blazor.Models
{
    public record CurrentWeather
    {
        [JsonPropertyName("coord")]
        public Coord Coord { get; set; } = null!;

        [JsonPropertyName("weather")]
        public IList<Weather> Weather { get; set; } = null!;

        [JsonPropertyName("base")]
        public string Base { get; set; } = null!;

        [JsonPropertyName("main")]
        public Main Main { get; set; } = null!;

        [JsonPropertyName("wind")]
        public Wind Wind { get; set; } = null!;

        [JsonPropertyName("rain")]
        public Main Rain { get; set; } = null!;

        [JsonPropertyName("clouds")]
        public Clouds Clouds { get; set; } = null!;

        [JsonPropertyName("dt")]
        public int Dt { get; set; }

        [JsonPropertyName("sys")]
        public Sys Sys { get; set; } = null!;

        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = null!;

        [JsonPropertyName("cod")]
        public int Cod { get; set; }
    }
}
