// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using Microsoft.JSInterop;

namespace Learning.Blazor.TwitterComponents.Extensions
{
    internal static class JSRuntimeExtensions
    {
        /// <summary>
        /// Renders Tweets, applying Twitter specific styling, links, etc.
        /// </summary>
        /// <param name="jsRuntime">The current <paramref name="jsRuntime"/> instance.</param>
        /// <returns>A <see cref="ValueTask"/> that represents the asynchronous rendering of tweets.</returns>
        /// <remarks>
        /// JavaScript interop which calls into, <c>window.twitter.renderTweets();</c>
        /// as part of the twitter-component.js.
        /// </remarks>
        internal static ValueTask RenderTweetsAsync(
            this IJSRuntime jsRuntime) =>
            jsRuntime!.InvokeVoidAsync("twitter.renderTweets");
    }
}
