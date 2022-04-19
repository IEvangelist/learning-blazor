// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using RecognitionError = Microsoft.JSInterop.SpeechRecognitionErrorEvent;

namespace Learning.Blazor.Components
{
    public sealed partial class AdditiveSpeechRecognitionComponent : IAsyncDisposable
    {
        IDisposable? _recognitionSubscription;
        RecognitionError? _error = null;
        bool _isRecognizing = false;

        string _dynamicCSS => _isRecognizing ? "is-flashing" : "";

        [Inject]
        private ISpeechRecognitionService SpeechRecognition { get; set; } = null!;

        [Parameter]
        public EventCallback SpeechRecognitionStarted { get; set; }

        [Parameter]
        public EventCallback<RecognitionError?> SpeechRecognitionStopped { get; set; }

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
                _recognitionSubscription?.Dispose();
                _recognitionSubscription = SpeechRecognition.RecognizeSpeech(
                    bcp47Tag,
                    OnRecognized,
                    OnError,
                    OnStarted,
                    OnEnded);
            }
        }

        void OnRecognized(string transcript) =>
            _ = SpeechRecognized.TryInvokeAsync(this, transcript);

        void OnError(RecognitionError recognitionError)
        {
            (_isRecognizing, _error) = (false, recognitionError);
            _ = SpeechRecognitionStopped.TryInvokeAsync(this, _error);
        }

        void OnStarted()
        {
            _isRecognizing = true;
            _ = SpeechRecognitionStarted.TryInvokeAsync(this);
        }

        public void OnEnded()
        {
            _isRecognizing = false;
            _ = SpeechRecognitionStopped.TryInvokeAsync(this, _error);
        }

        ValueTask IAsyncDisposable.DisposeAsync()
        {
            _recognitionSubscription?.Dispose();
            return SpeechRecognition.DisposeAsync();
        }
    }
}
