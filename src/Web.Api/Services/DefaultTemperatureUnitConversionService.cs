// Copyright (c) 2021 David Pine. All rights reserved.
//  Licensed under the MIT License.

using TempUom = Learning.Blazor.Models.TemperatureUnitOfMeasure;

namespace Learning.Blazor.Api.Services
{
    internal class DefaultTemperatureUnitConversionService
        : ITemperatureUnitConversionService
    {
        public double ConvertValue(double value,TempUom sourceUom, TempUom targetUom) =>
            (sourceUom, targetUom) switch
            {
                // °C to °F or °K
                (TempUom.Metric, TempUom.Imperial) => value * 1.8 + 32,
                (TempUom.Metric, TempUom.Standard) => value + 273.15,

                // °F to °C or °K
                (TempUom.Imperial, TempUom.Metric) => (value - 32) * 0.55,
                (TempUom.Imperial, TempUom.Standard) => (value - 32) * 5 / 9 + 273.15,

                // °K to °F or °C
                (TempUom.Standard, TempUom.Imperial) => (value - 273.15) * 9 / 5 + 32,
                (TempUom.Standard, TempUom.Metric) => value - 273.15,

                _ => value
            };
    }
}
