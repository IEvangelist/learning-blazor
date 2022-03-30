// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Components
{
    public sealed partial class AdditiveSpeechRecognitionComponent : IDisposable
    {
        readonly SpeechRecognitionService _speechRecognitionService;

        SpeechRecognitionError? _error = null;
        bool _isRecognizing = false;

        string _dynamicCSS => _isRecognizing ? "is-flashing" : "";

        [Inject]
        private IJSInProcessRuntime JavaScriptRuntime { get; set; } = null!;

        [Parameter]
        public EventCallback SpeechRecognitionStarted { get; set; }

        [Parameter]
        public EventCallback<SpeechRecognitionError?> SpeechRecognitionStopped { get; set; }

        [Parameter, EditorRequired]
        public EventCallback<string> SpeechRecognized { get; set; }

        public AdditiveSpeechRecognitionComponent() =>
            _speechRecognitionService =
                new SpeechRecognitionService(
                    async speechRecognition =>
                    {
                        if (SpeechRecognized.HasDelegate)
                        {
                            await SpeechRecognized.InvokeAsync(speechRecognition);
                        }
                    });

        void OnRecognizeButtonClick()
        {
            if (_isRecognizing)
            {
                JavaScriptRuntime.CancelSpeechRecognition(false);
            }
            else
            {
                var bcp47Tag = Culture.CurrentCulture.Name;
                JavaScriptRuntime.StartRecognizingSpeech(
                    this,
                    bcp47Tag,
                    nameof(OnStartedAsync),
                    nameof(OnEndedAsync),
                    nameof(OnErrorAsync),
                    nameof(OnRecognized));
            }
        }

        [JSInvokable]
        public Task OnStartedAsync() =>
            InvokeAsync(async () =>
            {
                _isRecognizing = true;

                if (SpeechRecognitionStarted.HasDelegate)
                {
                    await SpeechRecognitionStarted.InvokeAsync();
                }

                StateHasChanged();
            });

        [JSInvokable]
        public Task OnEndedAsync() =>
            InvokeAsync(async () =>
            {
                _isRecognizing = false;

                if (SpeechRecognitionStopped.HasDelegate)
                {
                    await SpeechRecognitionStopped.InvokeAsync(_error);
                }

                StateHasChanged();
            });

        [JSInvokable]
        public Task OnErrorAsync(SpeechRecognitionError recognitionError) =>
            InvokeAsync(async () =>
            {
                (_isRecognizing, _error) = (false, recognitionError);

                if (SpeechRecognitionStopped.HasDelegate)
                {
                    await SpeechRecognitionStopped.InvokeAsync(_error);
                }

                StateHasChanged();
            });
        
        [JSInvokable]
        public void OnRecognized(string transcript, bool isFinal) =>
            _speechRecognitionService.RecognitionReceived(
                new(transcript, isFinal));

        void IDisposable.Dispose() => _speechRecognitionService.Dispose();
    }
}
