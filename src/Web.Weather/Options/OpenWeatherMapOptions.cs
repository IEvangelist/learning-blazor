// Copyright (c) 2021 David Pine. All rights reserved.
//  Licensed under the MIT License.

namespace Web.Weather.Options
{
    public class OpenWeatherMapOptions
    {
        /// <summary>
        /// Get a free API key: https://openweathermap.org/
        /// </summary>
        public string ApiKey { get; set; }

        /// <summary>
        /// Defaults to: "https://api.openweathermap.org/data/2.5/"
        /// </summary>
        public string BaseApiUrl { get; set; } = "https://api.openweathermap.org/data/2.5/";

        internal void Deconstruct(out string apiKey, out string baseApiUrl) =>
            (apiKey, baseApiUrl) = (ApiKey, BaseApiUrl);
    }
}
