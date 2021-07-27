// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using Learning.Blazor.Models;

namespace Learning.Blazor.Api.Services
{
    /// <summary>
    /// See: https://openweathermap.org/api/one-call-api#multi
    /// </summary>
    public class WeatherLanguageService
    {
        /// <summary>
        /// See: https://openweathermap.org/api/one-call-api#multi
        /// </summary>
        internal ISet<WeatherLanguage> GetWeatherLanguages() => new HashSet<WeatherLanguage>
        {
            new("af","Afrikaans"),
            new("al", "Albanian"),
            new("ar", "Arabic"),
            new("az", "Azerbaijani"),
            new("bg", "Bulgarian"),
            new("ca", "Catalan"),
            new("cz", "Czech"),
            new("da", "Danish"),
            new("de", "German"),
            new("el", "Greek"),
            new("en", "English"),
            new("eu", "Basque"),
            new("fa", "Persian (Farsi)"),
            new("fi", "Finnish"),
            new("fr", "French"),
            new("gl", "Galician"),
            new("he", "Hebrew"),
            new("hi", "Hindi"),
            new("hr", "Croatian"),
            new("hu", "Hungarian"),
            new("id", "Indonesian"),
            new("it", "Italian"),
            new("ja", "Japanese"),
            new("kr", "Korean"),
            new("la", "Latvian"),
            new("lt", "Lithuanian"),
            new("mk", "Macedonian"),
            new("no", "Norwegian"),
            new("nl", "Dutch"),
            new("pl", "Polish"),
            new("pt", "Portuguese"),
            new("pt_br", "Português Brasil"),
            new("ro", "Romanian"),
            new("ru", "Russian"),
            new("sv", "Swedish"),
            new("sk", "Slovak"),
            new("sl", "Slovenian"),
            new("es", "Spanish"),
            new("sr", "Serbian"),
            new("th", "Thai"),
            new("tr", "Turkish"),
            new("uk", "Ukrainian"),
            new("vi", "Vietnamese"),
            new("zh_cn", "Chinese Simplified"),
            new("zh_tw", "Chinese Traditional"),
            new("zu", "Zulu")
        };
    }
}
