// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Web.Api")]

namespace Learning.Blazor.Models
{
    public enum MeasurementSystem
    {
        /// <summary>
        /// For example, the temperature in °F (degrees Fahrenheit), speed in miles per hour (MPH).
        /// </summary>
        Imperial = 0,

        /// <summary>
        /// For example, the temperature in °C (degrees Celcius), speed in kilometers per hour (KPH).
        /// </summary>
        Metric = 1
    }
}
