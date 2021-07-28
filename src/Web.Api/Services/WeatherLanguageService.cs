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
            //new("al", "Albanian"),    // "sq" in Azure
            new("ar", "Arabic"),
            new("az", "Azerbaijani"),
            new("bg", "Bulgarian"),
            new("ca", "Catalan"),
            //new("cz", "Czech"),       // "cs" in Azure
            new("da", "Danish"),
            new("de", "German"),
            new("el", "Greek"),
            new("en", "English"),
            //new("eu", "Basque"),
            new("fa", "Persian (Farsi)"),
            new("fi", "Finnish"),
            new("fr", "French"),
            //new("gl", "Galician"),
            new("he", "Hebrew"),
            new("hi", "Hindi"),
            new("hr", "Croatian"),
            new("hu", "Hungarian"),
            new("id", "Indonesian"),
            new("it", "Italian"),
            new("ja", "Japanese"),
            //new("kr", "Korean"),      // "ko" in Azure
            new("la", "Latvian"),
            new("lt", "Lithuanian"),
            //new("mk", "Macedonian"),
            new("no", "Norwegian"),     // "nb" in Azure
            new("nl", "Dutch"),
            new("pl", "Polish"),
            new("pt", "Portuguese"),            // "pt-PT" in Azure
            new("pt_br", "Português Brasil"),   // "pt" in Azure
            new("ro", "Romanian"),
            new("ru", "Russian"),
            new("sv", "Swedish"),
            new("sk", "Slovak"),
            new("sl", "Slovenian"),
            new("es", "Spanish"),
            //new("sr", "Serbian"),   // "sr-Cyrl" or "sr-Latn" in Azure
            new("th", "Thai"),
            new("tr", "Turkish"),
            new("uk", "Ukrainian"),
            new("vi", "Vietnamese"),
            new("zh_cn", "Chinese Simplified"),     // "zh-Hans"
            new("zh_tw", "Chinese Traditional"),    // "zh-Hant"
            //new("zu", "Zulu")
        };
    }
}
