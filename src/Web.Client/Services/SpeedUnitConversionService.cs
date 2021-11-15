// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using SpeedUom = Learning.Blazor.Models.MeasurementSystem;

namespace Learning.Blazor.Services;

internal class SpeedUnitConversionService
{
#pragma warning disable CA1822 // Mark members as static → used w/ DI
    public double ConvertValue(double value, SpeedUom sourceUom, SpeedUom targetUom) =>
#pragma warning restore CA1822 // Mark members as static
            (sourceUom, targetUom) switch
            {
                // KPH to MPH
                (SpeedUom.Metric, SpeedUom.Imperial) => value / 1.609344,

                // MPH to KPH
                (SpeedUom.Imperial, SpeedUom.Metric) => value * 1.609344,

                _ => value
            };
}
