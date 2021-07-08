﻿// Copyright (c) 2021 David Pine. All rights reserved.
//  Licensed under the MIT License.

namespace Learning.Blazor.Models
{
    /// <summary>
    /// OpenWeather API reference:
    ///   https://openweathermap.org/api/one-call-api#data
    /// </summary>
    public enum TemperatureUnitOfMeasure
    {
        /// <summary>
        /// Representing the temperature in °F (degrees Fahrenheit)
        /// </summary>
        Imperial = 0,

        /// <summary>
        /// Representing the temperature in °C (degrees Celcius)
        /// </summary>
        Metric = 1,

        /// <summary>
        /// Representing the temperature in °K (degrees Kelvin)
        /// </summary>
        Standard = 2
    }
}