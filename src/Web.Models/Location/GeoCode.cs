// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace Learning.Blazor.Models;

/// <summary>
/// See: https://www.bigdatacloud.com/geocoding-apis/free-reverse-geocode-to-city-api
/// </summary>
public record class GeoCode
{
    [JsonPropertyName("latitude")] public decimal Latitude { get; set; }
    [JsonPropertyName("longitude")] public decimal Longitude { get; set; }
    [JsonPropertyName("lookupSource")] public string? LookupSource { get; set; } = null!;
    [JsonPropertyName("plusCode")] public string? PlusCode { get; set; } = null!;
    [JsonPropertyName("localityLanguageRequested")] public string? LocalityLanguageRequested { get; set; } = null!;
    [JsonPropertyName("continent")] public string? Continent { get; set; } = null!;
    [JsonPropertyName("continentCode")] public string? ContinentCode { get; set; } = null!;
    [JsonPropertyName("countryName")] public string? CountryName { get; set; } = null!;
    [JsonPropertyName("countryCode")] public string? CountryCode { get; set; } = null!;
    [JsonPropertyName("principalSubdivision")] public string? PrincipalSubdivision { get; set; } = null!;
    [JsonPropertyName("principalSubdivisionCode")] public string? PrincipalSubdivisionCode { get; set; } = null!;
    [JsonPropertyName("city")] public string? City { get; set; } = null!;
    [JsonPropertyName("locality")] public string? Locality { get; set; } = null!;
    [JsonPropertyName("postcode")] public string? Postcode { get; set; } = null!;
}
