// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace Learning.Blazor.Models
{
    public record WeatherLanguage(
        [property: JsonPropertyName("twoLetterISO")] string TwoLetterISO,
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("bcp47Tag")] string Bcp47Tag);
}
