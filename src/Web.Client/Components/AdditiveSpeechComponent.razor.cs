// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Components
{
    public partial class AdditiveSpeechComponent
    {
        private bool _isSpeaking = false;
        private string _dynamicCSS
        {
            get
            {
                return string.Join(" ", GetStyles()).Trim();

                IEnumerable<string> GetStyles()
                {
                    if (string.IsNullOrWhiteSpace(Message))
                        yield return "is-hidden";

                    if (_isSpeaking)
                        yield return "is-flashing";
                };
            }
        }

        [Parameter]
        public string? Message { get; set; } = null!;

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
        public Task OnSpokenAsync(double elapsedTimeInMilliseconds) =>
            InvokeAsync(() =>
            {
                _isSpeaking = false;

                Logger.LogInformation(
                    "Spoke utterance in {ElapsedTime} milliseconds",
                    elapsedTimeInMilliseconds);

                StateHasChanged();
            });
    }
}
