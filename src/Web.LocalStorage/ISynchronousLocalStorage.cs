// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.LocalStorage;

public interface ISynchronousLocalStorage
{
    /// <inheritdoc cref="LocalStorageExtensions.Key(IJSInProcessRuntime, double)" />
    string? Key(double index);

    /// <inheritdoc cref="LocalStorageExtensions.Clear(IJSInProcessRuntime)" />
    void Clear();

    /// <inheritdoc cref="LocalStorageExtensions.RemoveItem(IJSInProcessRuntime, string)" />
    void Remove(string key);

    /// <inheritdoc cref="LocalStorageExtensions.GetItem(IJSInProcessRuntime, string)" />
    TItem? Get<TItem>(string key) where TItem : class;

    /// <inheritdoc cref="LocalStorageExtensions.SetItem(IJSInProcessRuntime, string, string)" />
    void Set<TItem>(string key, TItem item) where TItem : class;
}
