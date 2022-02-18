// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Extensions;

public static class JSInProcessRuntimeExtensions
{
    public static void StartRecognizingSpeech<TComponent>(
        this IJSInProcessRuntime jsRuntime,
        TComponent component,
        string language,
        string onStartMethodName,
        string onEndMethodName,
        string onErrorMethodName,
        string onRecognizedMethodName) where TComponent : class =>
        jsRuntime.InvokeVoid(
            "app.recognizeSpeech",
            DotNetObjectReference.Create(component),
            language,
            onStartMethodName,
            onEndMethodName,
            onErrorMethodName,
            onRecognizedMethodName);

    public static void CancelSpeechRecognition(
        this IJSInProcessRuntime jsRuntime,
        bool isAborted) =>
        jsRuntime.InvokeVoid(
            "app.cancelSpeechRecognition",
            isAborted);
}
