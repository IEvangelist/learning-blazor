// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Learning.Blazor.Services;

internal sealed class SpeechRecognitionService : IDisposable
{
    readonly Subject<RecognitionResult> _speechRecognitionSubject = new();
    readonly IObservable<string> _speechRecognitionObservable;
    readonly IDisposable _speechRecognitionSubscription;
    readonly Func<string, Task> _observer;


    internal SpeechRecognitionService(Func<string, Task> observer)
    {
        _observer = observer;
        _speechRecognitionObservable =
            _speechRecognitionSubject.AsObservable()
                .Where(recognition => recognition.IsFinal)
                .Select(recognition => recognition.Transcript);

        _speechRecognitionSubscription =
            _speechRecognitionObservable.Subscribe(
                async speechRecognition => await _observer(speechRecognition));
    }

    internal void RecognitionReceived(RecognitionResult recognition) =>
        _speechRecognitionSubject.OnNext(recognition);

    public void Dispose() => _speechRecognitionSubscription.Dispose();
}

internal readonly record struct RecognitionResult(string Transcript, bool IsFinal);
