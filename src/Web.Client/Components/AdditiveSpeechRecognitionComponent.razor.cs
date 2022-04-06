// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Components
{
    public sealed partial class AdditiveSpeechRecognitionComponent : IAsyncDisposable
    {
        SpeechRecognitionErrorEvent? _error = null;
        bool _isRecognizing = false;

        string _dynamicCSS => _isRecognizing ? "is-flashing" : "";

        [Inject]
        private ISpeechRecognitionService SpeechRecognition { get; set; } = null!;

        [Parameter]
        public EventCallback SpeechRecognitionStarted { get; set; }

        [Parameter]
        public EventCallback<SpeechRecognitionErrorEvent?> SpeechRecognitionStopped { get; set; }

        [Parameter, EditorRequired]
        public EventCallback<string> SpeechRecognized { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await SpeechRecognition.InitializeModuleAsync();
            }
        }

        void OnRecognizeButtonClick()
        {
            if (_isRecognizing)
            {
                SpeechRecognition.CancelSpeechRecognition(false);
            }
            else
            {
                var bcp47Tag = Culture.CurrentCulture.Name;
                SpeechRecognition.RecognizeSpeech(
                    bcp47Tag,
                    OnRecognized,
                    OnError,
                    OnStarted,
                    OnEnded);
            }
        }

        void OnRecognized(string transcript)
        {
            if (SpeechRecognized.HasDelegate)
            {
                SpeechRecognized.InvokeAsync(transcript);
            }
            StateHasChanged();
        }

        void OnError(SpeechRecognitionErrorEvent recognitionError)
        {
            (_isRecognizing, _error) = (false, recognitionError);
            if (SpeechRecognitionStopped.HasDelegate)
            {
                SpeechRecognitionStopped.InvokeAsync(_error);
            }
            StateHasChanged();
        }

        void OnStarted()
        {
            _isRecognizing = true;
            if (SpeechRecognitionStarted.HasDelegate)
            {
                SpeechRecognitionStarted.InvokeAsync();
            }
            StateHasChanged();
        }

        public void OnEnded()
        {
            _isRecognizing = false;
            if (SpeechRecognitionStopped.HasDelegate)
            {
                SpeechRecognitionStopped.InvokeAsync(_error);
            }
            StateHasChanged();
        }

        ValueTask IAsyncDisposable.DisposeAsync() =>
            SpeechRecognition.DisposeAsync();
    }
}
