// Copyright (c) 2021 David Pine. All rights reserved.
//  Licensed under the MIT License.

using System.Text.Json.Serialization;
using Learning.Blazor.Extensions;
using Learning.Blazor.Models;

namespace Learning.Blazor.Api.Requests
{
    public record WeatherRequest(
        [property: JsonPropertyName("lang")] string Language,
        [property: JsonPropertyName("lat")] decimal Latitude,
        [property: JsonPropertyName("lon")] decimal Longitude,
        [property: JsonPropertyName("units")] string Units)
    {
        public TemperatureUnitOfMeasure TemperatureUnitOfMeasure =>
            Units.ToEnum<TemperatureUnitOfMeasure>();

        internal string ToFormattedUrl(string weatherFunctionUrlFormat)
        {
            // http://localhost:7071/api/currentweather/{lang}/{latitude}/{longitude}/{units}

            return weatherFunctionUrlFormat
                .Replace("{lang}", Language)
                .Replace("{lat}", Latitude.ToString())
                .Replace("{lon}", Longitude.ToString())
                .Replace("{units}", TemperatureUnitOfMeasure.ToString());
        }
    }
}
