// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.BrowserModels;

public record SpeechRecognitionError(
    [property: JsonPropertyName("error")] string Error,
    [property: JsonPropertyName("message")] string Message);
