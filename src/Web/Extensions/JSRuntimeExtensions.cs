// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using Microsoft.JSInterop;

namespace Learning.Blazor.Extensions;

internal static class JSRuntimeExtensions
{
    internal static ValueTask GetCoordinatesAsync<T>(
        this IJSRuntime jsRuntime,
        T dotnetObj,
        string successMethodName,
        string errorMethodName) where T : class =>
        jsRuntime!.InvokeVoidAsync(
            "app.getClientCoordinates",
            DotNetObjectReference.Create(dotnetObj),
            successMethodName,
            errorMethodName);

    internal static async ValueTask<string> GetClientVoices<T>(
        this IJSRuntime javaScript,
        T dotnetObj,
        string callbackMethodName) where T : class =>
        await javaScript.InvokeAsync<string>(
            "app.getClientVoices",
            DotNetObjectReference.Create(dotnetObj),
            callbackMethodName);

    internal static async ValueTask<bool> GetCurrentDarkThemePreference<T>(
        this IJSRuntime javaScript,
        T dotnetObj,
        string callbackMethodName) where T : class =>
        await javaScript.InvokeAsync<bool>(
            "app.getClientPrefersColorScheme",
            "dark",
            DotNetObjectReference.Create(dotnetObj),
            callbackMethodName);

    internal static async ValueTask ScrollIntoViewAsync(
        this IJSRuntime javaScript,
        string selector) =>
        await javaScript.InvokeVoidAsync(
            "app.scrollIntoView", selector);
}
