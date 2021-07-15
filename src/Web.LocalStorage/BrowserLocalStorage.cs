// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Threading.Tasks;
using Learning.Blazor.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

namespace Learning.Blazor.LocalStorage
{
    /// <summary>
    /// See: https://developer.mozilla.org/en-US/docs/Web/API/Window/localStorage
    /// </summary>
    internal class BrowserLocalStorage : ILocalStorage
    {
        private readonly IJSRuntime _jSRuntime;
        private readonly ILogger<BrowserLocalStorage> _logger;

        public BrowserLocalStorage(IJSRuntime jSRuntime, ILogger<BrowserLocalStorage> logger)
        {
            _jSRuntime = jSRuntime;
            _logger = logger;
        }

        ValueTask ILocalStorage.ClearAsync() =>
            _jSRuntime.InvokeVoidAsync("localStorage.clear");

        ValueTask ILocalStorage.RemoveAsync(string key) =>
            _jSRuntime.InvokeVoidAsync("localStorage.removeItem", key);

        async ValueTask<TItem?> ILocalStorage.GetAsync<TItem>(string key) where TItem : class
        {
            var json = await _jSRuntime.InvokeAsync<string?>("localStorage.getItem", key);
            return json is { Length: > 0 } ? TryFromJson<TItem>(json) : default!;
        }

        ValueTask ILocalStorage.SetAsync<TItem>(string key, TItem item)
        {
            var json = TryToJson(item);
            return json is null
                ? ValueTask.CompletedTask
                : _jSRuntime.InvokeVoidAsync("localStorage.setItem", key, json);
        }

        private string? TryToJson<TItem>(TItem item)
        {
            try
            {
                return item.ToJson();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return default;
        }

        private TItem? TryFromJson<TItem>(string json) where TItem : class
        {
            try
            {
                return json.FromJson<TItem>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return default;
        }
    }
}
