// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Services;

public sealed class GeoLocationService(
    HttpClient httpClient,
    ILogger<GeoLocationService> logger)
{
    public async Task<GeoCode?> GetGeoCodeAsync(GeoCodeRequest request)
    {
        try
        {
            var (lat, lon, lang) = request;
            var queryString =
                $"?latitude={lat}&longitude={lon}&localityLanguage={lang}";

            var geoCode =
                await httpClient.GetFromJsonAsync<GeoCode>(
                    queryString,
                    DefaultJsonSerialization.Options);

            return geoCode;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
        }

        return null!;
    }
}
