// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace Learning.Blazor.Models;

/// <summary>
/// See: https://www.bigdatacloud.com/geocoding-apis/free-reverse-geocode-to-city-api
/// </summary>
public record class GeoCode
{
    [JsonPropertyName("latitude")] public decimal Latitude { get; init; }
    [JsonPropertyName("longitude")] public decimal Longitude { get; init; }
    [JsonPropertyName("lookupSource")] public string? LookupSource { get; init; } = null!;
    [JsonPropertyName("plusCode")] public string? PlusCode { get; init; } = null!;
    [JsonPropertyName("localityLanguageRequested")] public string? LocalityLanguageRequested { get; init; } = null!;
    [JsonPropertyName("continent")] public string? Continent { get; init; } = null!;
    [JsonPropertyName("continentCode")] public string? ContinentCode { get; init; } = null!;
    [JsonPropertyName("countryName")] public string? CountryName { get; init; } = null!;
    [JsonPropertyName("countryCode")] public string? CountryCode { get; init; } = null!;
    [JsonPropertyName("principalSubdivision")] public string? PrincipalSubdivision { get; init; } = null!;
    [JsonPropertyName("principalSubdivisionCode")] public string? PrincipalSubdivisionCode { get; init; } = null!;
    [JsonPropertyName("city")] public string? City { get; init; } = null!;
    [JsonPropertyName("locality")] public string? Locality { get; init; } = null!;
    [JsonPropertyName("postcode")] public string? Postcode { get; init; } = null!;
}
