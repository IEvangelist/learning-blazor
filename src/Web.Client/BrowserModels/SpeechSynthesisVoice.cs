// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.BrowserModels;

/// <summary>
/// https://developer.mozilla.org/docs/Web/API/SpeechSynthesisVoice
/// </summary>
public record class SpeechSynthesisVoice
{
    public bool Default { get; init; }
    public string Lang { get; init; } = null!;
    public string Name { get; init; } = null!;

    public override string ToString() => Name;
}
