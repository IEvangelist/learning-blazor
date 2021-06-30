// Copyright (c) 2021 David Pine. All rights reserved.
//  Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Learning.Blazor.Models;
using Learning.Blazor.Twitter.Extensions;
using Learning.Blazor.Twitter.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

namespace Learning.Blazor.Twitter.Components
{
    public sealed partial class TweetComponent : IAsyncDisposable
    {
        private readonly List<TweetContents> _tweets = new();
        private readonly ISet<string> _tracks =
            new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "#Blazor",
                "#WebAssembly",
                "C#",
                "@dotnet",
                "@davidpine7"
            };

        private StreamingStatus? _streamingStatus;
        private bool? _isStreaming => _streamingStatus is { /* "something" */ };

        [Inject]
        public IJSRuntime _javaScript { get; set; } = null!;

        [Inject]
        private ITwitterService _twitterService { get; set; } = null!;

        [Inject]
        private ILogger<TweetComponent> _logger { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            await _twitterService.StartTweetStreamAsync();

            _twitterService.StatusUpdated += OnStatusUpdated;
            _twitterService.TweetReceived += OnTweetReceived;
        }

        private Task OnStatusUpdated(StreamingStatus status) =>
            InvokeAsync(() =>
            {
                _streamingStatus = status;
                StateHasChanged();
            });

        private Task OnTweetReceived(TweetContents tweet) =>
            InvokeAsync(async () =>
            {
                _tweets?.Add(tweet);
                StateHasChanged();

                // We need to tell the Twitter HTML to render correctly.
                // This is a Twitter thing, not a Blazor thing...
                await _javaScript.RenderTwitterCardsAsync();
            });

        private void RemoveTrack(string track)
        {
            _ = _tracks.Remove(track);
            _twitterService.RemoveTrack(track);

            StateHasChanged();
        }

        private async Task Start() =>
            await _twitterService.StartTweetStreamAsync();

        private void Stop() =>
            _twitterService.StopTweetStream();

        private void Pause() =>
            _twitterService.PauseTweetStream();

        ValueTask IAsyncDisposable.DisposeAsync() =>
            _twitterService.DisposeAsync();
    }
}
