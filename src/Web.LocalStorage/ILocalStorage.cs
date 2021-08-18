// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.LocalStorage;

public interface ILocalStorage
{
    ValueTask ClearAsync();

    ValueTask RemoveAsync(string key);

    ValueTask<TItem?> GetAsync<TItem>(string key) where TItem : class;

    ValueTask SetAsync<TItem>(string key, TItem item);
}
