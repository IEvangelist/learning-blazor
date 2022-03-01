// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Services;

public class CultureService
{
    private readonly ILogger<CultureService> _logger;

    public CultureInfo CurrentCulture { get; } = CultureInfo.CurrentCulture;

    public RegionInfo CurrentRegion { get; } = new(CultureInfo.CurrentCulture.LCID);

    public MeasurementSystem MeasurementSystem =>
        CurrentRegion.IsMetric
            ? MeasurementSystem.Metric
            : MeasurementSystem.Imperial;

    public CultureService(ILogger<CultureService> logger) => _logger = logger;

    public string GetCultureTwoLetterRegionName(CultureInfo? culture = null) =>
        (culture is null ? CurrentRegion : new RegionInfo(culture.LCID))
            .TwoLetterISORegionName
            .ToLowerInvariant();

    public IDictionary<CultureInfo, AzureCulture> MapClientSupportedCultures(
        IDictionary<string, AzureCulture>? azureCultures)
    {
        HashSet<(CultureInfo Culture, AzureCulture AzureCulture)> supportedCultures = new();

        if (azureCultures is not null)
        {
            foreach (var group in CultureInfo.GetCultures(CultureTypes.SpecificCultures)
                    .GroupBy(culture => culture.TwoLetterISOLanguageName)
                    .Where(group => azureCultures.ContainsKey(group.Key)))
            {
                CultureInfo culture;
                var azureCulture = azureCultures[group.Key];

                if (group.Key == "en")
                {
                    culture = group.Single(c => c.Name == "en-US");
                    supportedCultures.Add((culture, azureCulture));
                    continue;
                }

                var cultures = group.ToList();
                if (cultures is { Count: 1 })
                {
                    supportedCultures.Add((cultures[0], azureCulture));
                    continue;
                }

                if (cultures is { Count: > 1 })
                {
                    var simpleCulture = $"{group.Key}-{group.Key.ToUpper()}";
                    culture = group.SingleOrDefault(c => c.Name == simpleCulture)!;
                    if (culture is not null)
                    {
                        supportedCultures.Add((culture, azureCulture));
                    }
                    else
                    {
                        _logger.LogInformation(
                            "Unable to find cultures for lang: {Key} - from: {Source}",
                            group.Key, string.Join(", ", cultures.Select(c => c.Name)));
                    }
                }
            }
        }

        return supportedCultures.ToDictionary(
            t => t.Culture,
            t => t.AzureCulture);
    }
}
