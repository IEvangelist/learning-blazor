// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace Learning.Blazor.BrowserModels
{
    public record ClientLocalePreference(
        [property: JsonPropertyName("twoLetterISO")] string TwoLetterISO,
        [property: JsonPropertyName("lcid")] int LCID);
}
