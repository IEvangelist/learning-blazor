// Copyright (c) 2021 David Pine. All rights reserved.
//  Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace Learning.Blazor.Models
{
    public record Wind
    {
        [JsonPropertyName("speed")]
        public double Speed { get; set; }

        [JsonPropertyName("deg")]
        public double Deg { get; set; }

        [JsonPropertyName("gust")]
        public double Gust { get; set; }
    }
}
