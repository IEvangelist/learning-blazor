// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Api.Services;

/// <summary>
/// See: https://openweathermap.org/api/one-call-api#multi
/// </summary>
public sealed class WeatherLanguageService
{
    /// <summary>
    /// See: https://openweathermap.org/api/one-call-api#multi
    /// </summary>
    internal ISet<WeatherLanguage> GetWeatherLanguages() => new HashSet<WeatherLanguage>
        {
            new("af","Afrikaans", "af"),
            new("al", "Albanian", "sq"),
            new("ar", "Arabic", "ar"),
            new("az", "Azerbaijani", "az"),
            new("bg", "Bulgarian", "bg"),
            new("ca", "Catalan", "ca"),
            new("cz", "Czech", "cs"),
            new("da", "Danish", "da"),
            new("de", "German", "de"),
            new("el", "Greek", "el"),
            new("en", "English", "en"),
            new("fa", "Persian (Farsi)", "fa"),
            new("fi", "Finnish", "fi"),
            new("fr", "French", "fr"),
            new("he", "Hebrew", "he"),
            new("hi", "Hindi", "hi"),
            new("hr", "Croatian", "hr"),
            new("hu", "Hungarian", "hu"),
            new("id", "Indonesian", "id"),
            new("it", "Italian", "it"),
            new("ja", "Japanese", "ja"),
            new("kr", "Korean", "ko"),
            new("la", "Latvian", "la"),
            new("lt", "Lithuanian", "lt"),
            new("mk", "Macedonian", ""),
            new("no", "Norwegian", "nb"),
            new("nl", "Dutch", "nl"),
            new("pl", "Polish", "pl"),
            new("pt", "Portuguese", "pt-PT"),
            new("pt_br", "Português Brasil", "pt"),
            new("ro", "Romanian", "ro"),
            new("ru", "Russian", "ru"),
            new("sv", "Swedish", "sv"),
            new("sk", "Slovak", "sk"),
            new("sl", "Slovenian", "sl"),
            new("es", "Spanish", "es"),
            new("sr", "Serbian", "sr-Cyrl"),
            new("th", "Thai", "th"),
            new("tr", "Turkish", "tr"),
            new("uk", "Ukrainian", "uk"),
            new("vi", "Vietnamese", "vi"),
            new("zh_cn", "Chinese Simplified", "zh-Hans"),
            new("zh_tw", "Chinese Traditional", "zh-Hant")
        };
}
