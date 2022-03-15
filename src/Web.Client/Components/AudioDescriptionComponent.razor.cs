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

        private AudioDescriptionModalComponent _modal = null!;
        private AudioDescriptionDetails _details;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                (_voice, _voiceSpeed) =
                    AppState.ClientVoicePreference;

                _details = new AudioDescriptionDetails(
                    AppState,
                    _voiceSpeeds,
                    _voices,
                    _voice,
                    _voiceSpeed);

                await UpdateClientVoices(
                    await JavaScript.GetClientVoices(
                        this, nameof(UpdateClientVoices)));
            }
        }

        [JSInvokable]
        public Task UpdateClientVoices(string voicesJson) =>
            InvokeAsync(() =>
            {
                var voices = voicesJson.FromJson<List<SpeechSynthesisVoice>>();
                if (voices is { Count: > 0 })
                {
                    _details = _details with
                    {
                        Voices = _voices = voices
                    };

                    StateHasChanged();
                }
            });

        private async Task ShowAsync() => await _modal.ShowAsync();

        private void OnDetailsSaved(AudioDescriptionDetails details)
        {
            // Noop...
            _details = details with { };

            AppState.ClientVoicePreference =
                new ClientVoicePreference(_details.Voice, _details.VoiceSpeed);

            Logger.LogInformation(
                "There are {Length} item in localStorage.", LocalStorage.Length);

        }
    }

    public readonly record struct AudioDescriptionDetails(
        AppInMemoryState AppState,
        IList<double> VoiceSpeeds,
        IList<SpeechSynthesisVoice> Voices,
        string Voice,
        double VoiceSpeed);
}
