// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace Learning.Blazor.Models
{
    /// <summary>
    /// See: https://www.bigdatacloud.com/geocoding-apis/free-reverse-geocode-to-city-api
    /// </summary>
    public record GeoCode(
        [property: JsonPropertyName("latitude")] decimal Latitude,
        [property: JsonPropertyName("longitude")] decimal Longitude,
        [property: JsonPropertyName("lookupSource")] string LookupSource,
        [property: JsonPropertyName("plusCode")] string PlusCode,
        [property: JsonPropertyName("localityLanguageRequested")] string LocalityLanguageRequested,
        [property: JsonPropertyName("continent")] string Continent,
        [property: JsonPropertyName("continentCode")] string ContinentCode,
        [property: JsonPropertyName("countryName")] string CountryName,
        [property: JsonPropertyName("countryCode")] string CountryCode,
        [property: JsonPropertyName("principalSubdivision")] string PrincipalSubdivision,
        [property: JsonPropertyName("principalSubdivisionCode")] string PrincipalSubdivisionCode,
        [property: JsonPropertyName("city")] string City,
        [property: JsonPropertyName("locality")] string Locality,
        [property: JsonPropertyName("postcode")] string Postcode);
}
