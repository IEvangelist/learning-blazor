// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using Learning.Blazor.Models;
using Microsoft.Extensions.Localization;

namespace Learning.Blazor.Services;

internal class WeatherStringFormatterService<T> : IWeatherStringFormatterService<T>
{
    private readonly IStringLocalizer<T> _localizer;
    private readonly CultureService _cultureService;
    private readonly TemperatureUnitConversionService _temperatureUnitConversionService;
    private readonly SpeedUnitConversionService _speedUnitConversionService;

    public WeatherStringFormatterService(
        IStringLocalizer<T> localizer,
        CultureService cultureService,
        TemperatureUnitConversionService temperatureUnitConversionService,
        SpeedUnitConversionService speedUnitConversionService)
    {
        _localizer = localizer;
        _cultureService = cultureService;
        _temperatureUnitConversionService = temperatureUnitConversionService;
        _speedUnitConversionService = speedUnitConversionService;
    }

    public object? GetFormat(Type? formatType)
        => formatType == typeof(ICustomFormatter) ? this : null;

    public string Format(string? format, object? arg, IFormatProvider? formatProvider)
    {
        if (!Equals(formatProvider))
        {
            return null!;
        }

        if (string.IsNullOrWhiteSpace(format)) format = "t";

        /*
            "t"     temperature
            "w"     wind short-hand
            "ww"    wind verbose
         */

        var target = _cultureService.MeasurementSystem;

        return format switch
        {
            // Temperatures
            "t" when arg is double temp =>
                ToLocalizedTemperature(temp, target),

            _ => null!
        };

        static string ToLocalizedTemperature(
            double temp,
            MeasurementSystem target)
        {
            var tempUom = target == MeasurementSystem.Imperial ? "°F" : "°C";

            // Examples:
            //     Imperial:    73°F
            //     Metric:      23°C
            return $"{(int)temp}{tempUom}";
        }
    }
}
