// Copyright (c) 2021 David Pine. All rights reserved.
//  Licensed under the MIT License.


using Learning.Blazor.Models;

namespace Learning.Blazor.Api.Services
{
    internal class DefaultTemperatureUnitConversionService
        : ITemperatureUnitConversionService
    {
        /// <summary>
        /// Some formulas found in:
        ///   https://www.realifewebdesigns.com/web-programming/convert-metric-imperial.asp
        /// </summary>
        public double ConvertValue(
            double value,
            TemperatureUnitOfMeasure sourceUom,
            TemperatureUnitOfMeasure targetUom) => (sourceUom, targetUom) switch
            {
                (TemperatureUnitOfMeasure.Metric, TemperatureUnitOfMeasure.Imperial) => value * 1.8 + 32,
                (TemperatureUnitOfMeasure.Metric, TemperatureUnitOfMeasure.Standard) => value,

                (TemperatureUnitOfMeasure.Imperial, TemperatureUnitOfMeasure.Metric) => (value - 32) * 0.55,
                (TemperatureUnitOfMeasure.Imperial, TemperatureUnitOfMeasure.Standard) => value,

                (TemperatureUnitOfMeasure.Standard, TemperatureUnitOfMeasure.Imperial) => value,
                (TemperatureUnitOfMeasure.Standard, TemperatureUnitOfMeasure.Metric) => value,

                _ => value
            };
    }
}
