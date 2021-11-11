﻿// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using Learning.Blazor.BrowserModels;
using Learning.Blazor.Extensions;
using Learning.Blazor.Models;
using Learning.Blazor.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Learning.Blazor.Components
{
    public sealed partial class AudioDescriptionComponent
    {
        private readonly IList<double> _voiceSpeeds =
            Enumerable.Range(0, 12).Select(i => (i + 1) * .25).ToList();

        private IList<SpeechSynthesisVoice> _voices = null!;
        private string _voice = "Auto";
        private double _voiceSpeed = 1;

        private ModalComponent _modal = null!;

        [Inject]
        public AppInMemoryState AppState { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            AppState.ClientVoicePreference =
                await LocalStorage.GetAsync<ClientVoicePreference>(StorageKeys.ClientVoice);
            if (AppState.ClientVoicePreference is not null)
            {
                (_voice, _voiceSpeed) = AppState.ClientVoicePreference;
            }

            await UpdateClientVoices(
                await JavaScript.GetClientVoices(
                    this, nameof(UpdateClientVoices)));
        }

        [JSInvokable]
        public Task UpdateClientVoices(string voicesJson) =>
            InvokeAsync(() =>
            {
                var voices = voicesJson.FromJson<List<SpeechSynthesisVoice>>();
                if (voices is { Count: > 0 })
                {
                    _voices = voices;

                    StateHasChanged();
                }
            });

        private void OnVoiceSpeedChange(ChangeEventArgs args) =>
            _voiceSpeed = double.TryParse(
                args?.Value?.ToString() ?? "1", out var speed) ? speed : 1;

        private async Task Show() => await _modal.Show();

        private async Task Confirm()
        {
            AppState.ClientVoicePreference =
                new ClientVoicePreference(_voice, _voiceSpeed);

            await LocalStorage.SetAsync(
                StorageKeys.ClientVoice, AppState.ClientVoicePreference);

            await _modal.Confirm();
        }

        private void OnDismissed(DismissalReason reason) =>
            Logger.LogWarning("User '{Reason}' the audio description modal.", reason);
    }
}
