// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using TempUom = Learning.Blazor.Models.MeasurementSystem;

namespace Learning.Blazor.Services
{
    internal class TemperatureUnitConversionService
    {
#pragma warning disable CA1822 // Mark members as static → used w/ DI
        public double ConvertValue(double value, TempUom sourceUom, TempUom targetUom) =>
#pragma warning restore CA1822 // Mark members as static
            (sourceUom, targetUom) switch
            {
                // °C to °F
                (TempUom.Metric, TempUom.Imperial) => value * 1.8 + 32,

                // °F to °C
                (TempUom.Imperial, TempUom.Metric) => (value - 32) * 0.55,

                _ => value
            };
    }
}
