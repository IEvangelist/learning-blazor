﻿// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using Learning.Blazor.Extensions;
using Learning.Blazor.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Learning.Blazor.Components
{
    public partial class AdditiveSpeechComponent
    {
        private bool _isSpeaking = false;
        private string _hiddenCss => Message is null or { Length: 0 } ? "is-hidden" : "";

        [Parameter]
        public string Message { get; set; } = null!;

        [Inject]
        public AppInMemoryState AppState { get; set; } = null!;

        async Task OnSpeakButtonClickAsync()
        {
            if (Message is null or { Length: 0 })
            {
                return;
            }

            var (voice, voiceSpeed) = AppState.ClientVoicePreference;
            var bcp47Tag = Culture.CurrentCulture.Name;

            _isSpeaking = true;

            await JavaScript.SpeakMessageAsync(
                this,
                nameof(OnSpokenAsync),
                Message,
                voice,
                voiceSpeed,
                bcp47Tag);
        }

        [JSInvokable]
        public Task OnSpokenAsync(double elapsedTime) =>
            InvokeAsync(() =>
            {
                _isSpeaking = false;

                StateHasChanged();
            });

    }
}
