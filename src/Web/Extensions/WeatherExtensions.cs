// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using Learning.Blazor.Models;

namespace Learning.Blazor.Extensions
{
    internal static class WeatherExtensions
    {
        /// <summary>
        /// https://openweathermap.org/weather-conditions#Weather-Condition-Codes-2
        /// </summary>
        private static Dictionary<string, string> s_iconToImageMap =>
                new(StringComparer.OrdinalIgnoreCase)
                {
                    // Day            Night
                    { "01d", "01" }, { "01n", "33" },
                    { "02d", "03" }, { "02n", "35" },
                    { "03d", "06" }, { "03n", "38" },
                    { "04d", "07" }, { "04n", "08" },
                    { "09d", "12" }, { "09n", "12" },
                    { "10d", "13" }, { "10n", "39" },
                    { "11d", "15" }, { "11n", "41" },
                    { "13d", "22" }, { "13n", "44" },
                    { "50d", "05" }, { "50n", "35" }
                };

        internal static string ToImageName(this WeatherWordingAndIcon weather) =>
            s_iconToImageMap.TryGetValue(
                weather.Icon, out var value)
            ? value : "";
    }
}
