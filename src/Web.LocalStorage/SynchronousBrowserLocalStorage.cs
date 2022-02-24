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
        IJSInProcessRuntime jSRuntime,
        ILogger<SynchronousBrowserLocalStorage> logger) =>
        (_jSRuntime, _logger) = (jSRuntime, logger);

    void ISynchronousLocalStorage.Clear() =>
        _jSRuntime.Clear();

    void ISynchronousLocalStorage.Remove(string key) =>
        _jSRuntime.RemoveItem(key);

    TItem? ISynchronousLocalStorage.Get<TItem>(string key)
        where TItem : class =>
        _jSRuntime.GetItem(key) switch
        {
            { Length: > 0 } json => json.TryFromJson<TItem>(_logger),
            _ => default
        };

    void ISynchronousLocalStorage.Set<TItem>(string key, TItem item)
        where TItem : class
    {
        if (item.TryToJson(_logger) is { } json)
        {
            _jSRuntime.SetItem(key, json);
        }
    }
}
