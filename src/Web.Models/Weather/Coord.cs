// Copyright (c) 2021 David Pine. All rights reserved.
//  Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace Learning.Blazor.Models
{
    public record Coord
    {
        [JsonPropertyName("lon")]
        public decimal Lon { get; set; }

        [JsonPropertyName("lat")]
        public decimal Lat { get; set; }
    }
}
