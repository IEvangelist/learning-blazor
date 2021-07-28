// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace Learning.Blazor.BrowserModels
{
    public record ClientVoicePreference(
        [property: JsonPropertyName("voice")] string Voice,
        [property: JsonPropertyName("voiceSpeed")] double VoiceSpeed);
}
