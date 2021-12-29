// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.LocalStorage;

public interface ISynchronousLocalStorage
{
    void Clear();

    void Remove(string key);

    TItem? Get<TItem>(string key) where TItem : class;

    void Set<TItem>(string key, TItem item) where TItem : class;
}
