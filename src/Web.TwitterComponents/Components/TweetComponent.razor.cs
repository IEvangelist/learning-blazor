// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Learning.Blazor.Models;
using Learning.Blazor.TwitterComponents.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

namespace Learning.Blazor.TwitterComponents.Components
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

        private bool _showTracksModal = false;
        private StreamingStatus? _streamingStatus;
        private bool? _isStreaming => _streamingStatus is { /* "something" */ };
        private HubConnection _hubConnection = null!;

        [Inject]
        private IJSRuntime _javaScript { get; set; } = null!;

        [Inject]
        private NavigationManager _navigationManager { get; set; } = null!;

        [Inject]
        private ILogger<TweetComponent> _logger { get; set; } = null!;

        [Inject]
        private IAccessTokenProvider _tokenProvider { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            await Task.Delay(TimeSpan.FromSeconds(5));


            await OnTweetReceived(new Notification<TweetContents>(NotificationType.Tweet, new TweetContents
            {
                HTML = @"<blockquote class=""twitter-tweet"" style=""width: 400px;"" data-dnt=""true"">
<p lang=""en"" dir=""ltr""></p>

<a href=""https://twitter.com/davidpine7/status/1410259973519597570""></a>

</blockquote>"
            }));

            // _hubConnection = new HubConnectionBuilder()
            //     .WithUrl(new Uri("https://localhost:5002/notifictions")/*,
            //         options => options.AccessTokenProvider = GetAccessTokenValueAsync*/)
            //     .WithAutomaticReconnect()
            //     .AddMessagePackProtocol()
            //     .Build();
            // 
            // _hubConnection.On<Notification<StreamingStatus>>("StatusUpdated", OnStatusUpdated);
            // _hubConnection.On<Notification<TweetContents>>("TweetReceived", OnTweetReceived);
            // 
            // await _hubConnection.StartAsync();
            // await _hubConnection.InvokeAsync("StartTweetStream");
        }

        private async Task<string?> GetAccessTokenValueAsync()
        {
            var result = await _tokenProvider.RequestAccessToken();
            return result.TryGetToken(out var accessToken) ? accessToken.Value : null;
        }

        private Task OnStatusUpdated(Notification<StreamingStatus> status) =>
            InvokeAsync(() =>
            {
                _streamingStatus = status;
                StateHasChanged();
            });

        private Task OnTweetReceived(Notification<TweetContents> tweet) =>
            InvokeAsync(async () =>
            {
                _tweets?.Add(tweet);

                // We need to tell the Twitter HTML to render correctly.
                // This is a Twitter thing, not a Blazor thing...
                // We interop with JavaScript, calling functions from our .NET code.
                await _javaScript.RenderTweetsAsync();

                StateHasChanged();
            });

        private async Task RemoveTrack(string track)
        {
            _ = _tracks.Remove(track);

            await _hubConnection.InvokeAsync("RemoveTrack", track);
            await InvokeAsync(StateHasChanged);
        }

        private Task Start() =>
            _hubConnection.InvokeAsync("StartTweetStream");

        private Task Stop() =>
            _hubConnection.InvokeAsync("StopTweetStream");

        private Task Pause() =>
            _hubConnection.InvokeAsync("PauseTweetStream");

        ValueTask IAsyncDisposable.DisposeAsync() =>
            _hubConnection.DisposeAsync();
    }
}
