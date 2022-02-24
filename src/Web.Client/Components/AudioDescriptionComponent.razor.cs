// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

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

        protected override async Task OnInitializedAsync()
        {
            var clientVoicePreference =
                LocalStorage.Get<ClientVoicePreference>(StorageKeys.ClientVoice);
            if (clientVoicePreference is not null)
            {
                (_voice, _voiceSpeed) =
                    AppState.ClientVoicePreference = clientVoicePreference;
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

        private async Task ShowAsync() => await _modal.ShowAsync();

        private async Task ConfirmAsync()
        {
            AppState.ClientVoicePreference =
                new ClientVoicePreference(_voice, _voiceSpeed);

            LocalStorage.Set(
                StorageKeys.ClientVoice, AppState.ClientVoicePreference);

            await _modal.ConfirmAsync();
        }

        private void OnDismissed(DismissalReason reason) =>
            Logger.LogWarning(
                "User '{Reason}' the audio description modal.",
                reason);
    }
}
