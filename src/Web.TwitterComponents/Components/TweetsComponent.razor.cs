// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Threading.Tasks;
using Learning.Blazor.Models;
using Learning.Blazor.TwitterComponents.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Learning.Blazor.TwitterComponents.Components
{
    public sealed partial class TweetsComponent
    {
        [Inject]
        public IJSRuntime JavaScript { get; set; } = null!;

        [Parameter]
        public TweetContents[] Tweets { get; set; } = Array.Empty<TweetContents>();

        [Parameter]
        public string? Filter { get; set; } = null!;

        protected override async Task OnParametersSetAsync() =>
            await JavaScript.RenderTweetsAsync();

        private static bool TweetMatchesFilter(string? filter, TweetContents tweet)
        {
            if (filter is null or { Length: 0 })
            {
                return true;
            }

            if (tweet is null)
            {
                return false;
            }

            return tweet.AuthorName.Contains(filter, StringComparison.OrdinalIgnoreCase)
                || tweet.HTML.Contains(filter, StringComparison.OrdinalIgnoreCase);
        }
    }
}
