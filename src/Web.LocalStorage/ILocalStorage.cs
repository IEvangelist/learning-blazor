// Copyright (c) 2021 David Pine. All rights reserved.
//  Licensed under the MIT License.

using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Learning.Blazor.LocalStorage
{
    public interface ILocalStorage
    {
        ValueTask ClearAsync();

        ValueTask RemoveAsync(string key);

        [return: MaybeNull]
        ValueTask<TItem> GetAsync<TItem>(string key) where TItem : class;

        ValueTask SetAsync<TItem>(string key, TItem item);
    }
}
