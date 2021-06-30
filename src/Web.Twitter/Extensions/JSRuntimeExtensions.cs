// Copyright (c) 2021 David Pine. All rights reserved.
//  Licensed under the MIT License.

using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Learning.Blazor.Twitter.Extensions
{
    internal static class JSRuntimeExtensions
    {
        internal static ValueTask RenderTwitterCardsAsync(
            this IJSRuntime jsRuntime) =>
            jsRuntime!.InvokeVoidAsync("twitter.renderTwitterCards");
    }
}
