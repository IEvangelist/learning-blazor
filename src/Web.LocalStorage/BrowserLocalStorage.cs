// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.LocalStorage;

/// <summary>
/// See <a href="https://developer.mozilla.org/docs/Web/API/Window/localStorage"></a>
/// </summary>
internal sealed class BrowserLocalStorage : ILocalStorage
{
    private readonly IJSRuntime _jSRuntime;
    private readonly ILogger<BrowserLocalStorage> _logger;

    public BrowserLocalStorage(
        IJSRuntime jSRuntime, ILogger<BrowserLocalStorage> logger) =>
        (_jSRuntime, _logger) = (jSRuntime, logger);

    ValueTask ILocalStorage.ClearAsync() =>
        _jSRuntime.InvokeVoidAsync(NativeLocalStorageWebApi.Clear);

    ValueTask ILocalStorage.RemoveAsync(string key) =>
        _jSRuntime.InvokeVoidAsync(NativeLocalStorageWebApi.RemoveItem, key);

    async ValueTask<TItem?> ILocalStorage.GetAsync<TItem>(string key)
        where TItem : class =>
        await _jSRuntime.InvokeAsync<string?>(
            NativeLocalStorageWebApi.GetItem, key) switch
        {
            { Length: > 0 } json => json.TryFromJson<TItem>(_logger),
            _ => default
        };

    ValueTask ILocalStorage.SetAsync<TItem>(string key, TItem item)
        where TItem : class =>
        item.TryToJson(_logger) is { } json
            ? _jSRuntime.InvokeVoidAsync(
                NativeLocalStorageWebApi.SetItem, key, json)
            : ValueTask.CompletedTask;
}
