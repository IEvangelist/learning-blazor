// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.TwitterComponents.Extensions;

internal static class JSObjectReferenceExtensions
{
    /// <summary>
    /// Renders Tweets, applying Twitter specific styling, links, etc.
    /// </summary>
    /// <param name="jsModule">The current <paramref name="jsModule"/> instance.</param>
    /// <returns>A <see cref="ValueTask"/> that represents the asynchronous rendering of tweets.</returns>
    /// <remarks>
    /// JavaScript interop which calls into, <c>window.twitter.renderTweets();</c>
    /// as part of the twitter-component.js.
    /// </remarks>
    internal static ValueTask RenderTweetsAsync(
        this IJSObjectReference jsModule) =>
        jsModule.InvokeVoidAsync("renderTweets");
}
