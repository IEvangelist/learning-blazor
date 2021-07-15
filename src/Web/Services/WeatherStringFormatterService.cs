﻿// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using Learning.Blazor.Extensions;
using Learning.Blazor.ComponentModels;
using Learning.Blazor.Models;
using Microsoft.Extensions.Localization;
using System;

namespace Learning.Blazor.Services
{
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

            var source = MeasurementSystem.Imperial;
            var target = _cultureService.MeasurementSystem;

            return format switch
            {
                // Temperatures
                "t" when arg is double temp =>
                    ToLocalizedTemperature(
                        temp, source, target, _temperatureUnitConversionService),

                // Wind speeds
                //"w" => ToLocalizedWindSpeed(model.WindSpeed, model.WindDegree, source, target, _localizer, _speedUnitConversionService, false),
                //"ww" => ToLocalizedWindSpeed(model.WindSpeed, model.WindDegree, source, target, _localizer, _speedUnitConversionService, true),

                _ => null!
            };

            static string ToLocalizedTemperature(
                double temp,
                MeasurementSystem source,
                MeasurementSystem target,
                TemperatureUnitConversionService temperatureUnitConversionService)
            {
                var tempUom = target == MeasurementSystem.Imperial ? "℉" : "℃";

                // Examples:
                //     Imperial:    73 °F
                //     Metric:      23 °C
                return $"{temperatureUnitConversionService.ConvertValue(temp, source, target)} {tempUom}";
            }

            static string ToLocalizedWindSpeed(
                double speed,
                int degrees,
                MeasurementSystem source,
                MeasurementSystem target,
                IStringLocalizer<T> localizer,
                SpeedUnitConversionService speedUnitConversionService,
                bool isVerbose)
            {
                var speedUom = target == MeasurementSystem.Imperial ? "MPH" : "KPH";
                var cardinal = isVerbose ? degrees.ToVerboseCardinal() : degrees.ToCardinal();

                // Examples:
                //     Verbose:     7 MPH to the Northwest
                //     Short-hand:  7 NW
                return isVerbose
                    ? localizer[
                        $"{{0}} {speedUom} to the {cardinal}",
                        speedUnitConversionService.ConvertValue(speed, source, target)]
                    : localizer[
                        $"{{0}} {cardinal}",
                        speedUnitConversionService.ConvertValue(speed, source, target)];
            }
        }
    }
}
