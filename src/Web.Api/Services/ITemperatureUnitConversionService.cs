// Copyright (c) 2021 David Pine. All rights reserved.
//  Licensed under the MIT License.

using Learning.Blazor.Models;

namespace Learning.Blazor.Api.Services
{
    public interface ITemperatureUnitConversionService
    {
        double ConvertValue(
            double value,
            TemperatureUnitOfMeasure sourceUom,
            TemperatureUnitOfMeasure targetUom);
    }
}
