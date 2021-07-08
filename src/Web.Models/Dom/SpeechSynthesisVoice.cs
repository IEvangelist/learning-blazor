// Copyright (c) 2021 David Pine. All rights reserved.
//  Licensed under the MIT License.

namespace Learning.Blazor.Models
{
    /// <summary>
    /// https://developer.mozilla.org/en-US/docs/Web/API/SpeechSynthesisVoice
    /// </summary>
    public class SpeechSynthesisVoice
    {
        public bool Default { get; init; }
        public string Lang { get; init; } = null!;
        public string Name { get; init; } = null!;

        public override string ToString() => Name;
    }
}
