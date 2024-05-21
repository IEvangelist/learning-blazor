// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.TwitterComponents.Components;

public sealed partial class TweetsComponent : IAsyncDisposable
{
    private IJSObjectReference? _twitterModule;
    private TweetContents[] _filteredTweets =>
        Tweets?.Where(TweetMatchesFilter).ToArray() ??
        [];

    [Inject]
    public IJSRuntime JavaScript { get; set; } = null!;

    [Parameter]
    public TweetContents[] Tweets { get; set; } = [];

    [Parameter]
    public string? Filter { get; set; } = null!;

    [Parameter]
    public RenderFragment Loading { get; set; } = null!;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _twitterModule =
                await JavaScript.InvokeAsync<IJSObjectReference>(
                    "import",
                    "./_content/Web.TwitterComponents/twitter-component.js");
        }
    }

    protected override Task OnParametersSetAsync() =>
        InvokeAsync(async () =>
        {
            if (_twitterModule is not null)
            {
                await _twitterModule.RenderTweetsAsync();
            }
        });

    private bool TweetMatchesFilter(TweetContents tweet)
    {
        if (Filter is null or { Length: 0 })
        {
            return true;
        }

        if (tweet is null)
        {
            return false;
        }

        return IgnoreCaseContains(tweet.AuthorName, Filter)
            || IgnoreCaseContains(tweet.HTML, Filter);

        static bool IgnoreCaseContains(string? text, string filter) =>
            text?.Contains(filter, StringComparison.OrdinalIgnoreCase)
            ?? false;
    }

    async ValueTask IAsyncDisposable.DisposeAsync()
    {
        if (_twitterModule is not null)
        {
            await _twitterModule.DisposeAsync();
        }
    }
}
