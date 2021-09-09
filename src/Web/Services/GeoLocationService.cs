// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System.Net.Http.Json;
using Learning.Blazor.Models;
using Learning.Blazor.Serialization;

namespace Learning.Blazor.Services;

public sealed class GeoLocationService
{
    private readonly HttpClient _httpClient = null!;
    private readonly ILogger<GeoLocationService> _logger = null!;

    public GeoLocationService(
        HttpClient httpClient,
        ILogger<GeoLocationService> logger) =>
        (_httpClient, _logger) = (httpClient, logger);

    public async Task<GeoCode?> GetGeoCodeAsync(GeoCodeRequest request)
    {
        try
        {
            var (lat, lon, lang) = request;
            var queryString =
                $"?latitude={lat}&longitude={lon}&localityLanguage={lang}";

            var geoCode =
                await _httpClient.GetFromJsonAsync<GeoCode>(
                    queryString,
                    GeoCodeJsonSerializerContext.DefaultTypeInfo);

            return geoCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }

        return null!;
    }
}
