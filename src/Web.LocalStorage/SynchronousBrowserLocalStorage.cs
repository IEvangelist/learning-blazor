// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.LocalStorage;

/// <summary>
/// See <a href="https://developer.mozilla.org/docs/Web/API/Window/localStorage"></a>
/// </summary>
internal sealed class SynchronousBrowserLocalStorage : ISynchronousLocalStorage
{
    private readonly IJSInProcessRuntime _jSRuntime;
    private readonly ILogger<SynchronousBrowserLocalStorage> _logger;

    public SynchronousBrowserLocalStorage(
        IJSInProcessRuntime jSRuntime, ILogger<SynchronousBrowserLocalStorage> logger) =>
        (_jSRuntime, _logger) = (jSRuntime, logger);

    void ISynchronousLocalStorage.Clear() =>
        _jSRuntime.InvokeVoid(NativeLocalStorageWebApi.Clear);

    void ISynchronousLocalStorage.Remove(string key) =>
        _jSRuntime.InvokeVoid(NativeLocalStorageWebApi.RemoveItem, key);

    TItem? ISynchronousLocalStorage.Get<TItem>(string key)
        where TItem : class
    {
        var json =
            _jSRuntime.Invoke<string?>(
                NativeLocalStorageWebApi.GetItem, key);

        return json switch
        {
            { Length: > 0 } => json.TryFromJson<TItem>(_logger),
            _ => default
        };
    }

    void ISynchronousLocalStorage.Set<TItem>(string key, TItem item)
        where TItem : class
    {
        var json = item.TryToJson(_logger);
        if (json is not null)
        {
            _jSRuntime.InvokeVoid(
                NativeLocalStorageWebApi.SetItem, key, json);
        }
    }
}
