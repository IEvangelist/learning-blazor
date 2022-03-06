// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace Learning.Blazor.Models;

public record class WeatherLanguage
{
    [JsonPropertyName("weatherLanguageId")] public string? WeatherLanguageId { get; set; } = null!;
    [JsonPropertyName("name")] public string? Name { get; set; } = null!;
    [JsonPropertyName("azureCultureId")] public string? AzureCultureId { get; set; } = null!;

    public WeatherLanguage() { }

    public WeatherLanguage(
        string weatherLanguageId,
        string name,
        string azureCultureId)
    {
        WeatherLanguageId = weatherLanguageId;
        Name = name;
        AzureCultureId = azureCultureId;
    }
}
