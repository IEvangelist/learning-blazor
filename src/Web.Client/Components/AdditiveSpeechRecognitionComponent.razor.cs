// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Components
{
    public partial class AdditiveSpeechRecognitionComponent
    {
        private SpeechRecognitionError? _error = null;
        private bool _isRecognizing = false;

        private string _dynamicCSS => _isRecognizing ? "is-flashing" : "";

        [Inject]
        private IJSInProcessRuntime JavaScriptRuntime { get; set; } = null!;

        [Parameter, EditorRequired]
        public EventCallback<(string Transcript, bool IsFinal)> SpeechRecognized { get; set; }

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
                    nameof(OnRecognizedAsync));
            }
        }

        [JSInvokable]
        public Task OnStartedAsync() =>
            InvokeAsync(() =>
            {
                _isRecognizing = true;
                StateHasChanged();
            });

        [JSInvokable]
        public Task OnEndedAsync() =>
            InvokeAsync(() =>
            {
                _isRecognizing = false;
                StateHasChanged();
            });

        [JSInvokable]
        public Task OnErrorAsync(SpeechRecognitionError recognitionError) =>
            InvokeAsync(() =>
            {
                (_isRecognizing, _error) = (false, recognitionError);
                StateHasChanged();
            });

        [JSInvokable]
        public async Task OnRecognizedAsync(string transcript, bool isFinal)
        {
            if (SpeechRecognized.HasDelegate)
            {
                await SpeechRecognized.InvokeAsync((transcript, isFinal));
            }
        }
    }
}
