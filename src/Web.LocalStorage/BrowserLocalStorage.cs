// Copyright (c) 2021 David Pine. All rights reserved.
//  Licensed under the MIT License.

using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Learning.Blazor.Extensions;
using Microsoft.JSInterop;

namespace Learning.Blazor.LocalStorage
{
    /// <summary>
    /// See: https://developer.mozilla.org/en-US/docs/Web/API/Window/localStorage
    /// </summary>
    internal class BrowserLocalStorage : ILocalStorage
    {
        private readonly IJSRuntime _jSRuntime;

        public BrowserLocalStorage(IJSRuntime jSRuntime) => _jSRuntime = jSRuntime;

        ValueTask ILocalStorage.ClearAsync() =>
            _jSRuntime.InvokeVoidAsync("localStorage.clear");

        ValueTask ILocalStorage.RemoveAsync(string key) =>
            _jSRuntime.InvokeVoidAsync("localStorage.removeItem", key);

        [return: MaybeNull]
        async ValueTask<TItem> ILocalStorage.GetAsync<TItem>(string key) where TItem : class
        {
            string? json = await _jSRuntime.InvokeAsync<string?>("localStorage.getItem", key);
            if (json is { Length: > 0 })
            {
                return json.FromJson<TItem>()!;
            }

            return default!;
        }

        ValueTask ILocalStorage.SetAsync<TItem>(string key, TItem item) =>
            _jSRuntime.InvokeVoidAsync("localStorage.setItem", key, item.ToJson() ?? "");
    }
}
