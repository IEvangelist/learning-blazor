// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Learning.Blazor.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Learning.Blazor.Pages
{
    public sealed partial class Tweets : IAsyncDisposable
    {
        private HashSet<TweetContents> _tweets = new();
        private StreamingStatus? _streamingStatus;
        private HubConnection _hubConnection = null!;

        private string _streamingFontAwesomeClass =>
            _hubConnection is { State: HubConnectionState.Connected } &&
            _streamingStatus is { IsStreaming: true }
                ? "fas fa-link has-text-primary"
                : "fas fa-unlink has-text-warning";

        [Inject]
        public IAccessTokenProvider TokenProvider { get; set; } = null!;

        [Inject]
        public IConfiguration Config { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            var notificationHub =
                new Uri($"{Config["WebApiServerUrl"]}/notifications");
            _hubConnection = new HubConnectionBuilder()
                .WithUrl(notificationHub,
                     options => options.AccessTokenProvider = GetAccessTokenValueAsync)
                .WithAutomaticReconnect()
                .AddMessagePackProtocol()
                .Build();

            _hubConnection.On<Notification<StreamingStatus>>("StatusUpdated", OnStatusUpdated);
            _hubConnection.On<Notification<TweetContents>>("TweetReceived", OnTweetReceived);

            await _hubConnection.StartAsync();

            await _hubConnection.InvokeAsync("JoinTweets");
            await _hubConnection.InvokeAsync("StartTweetStream");
        }

        private async Task<string?> GetAccessTokenValueAsync()
        {
            var result = await TokenProvider.RequestAccessToken();
            return result.TryGetToken(out var accessToken) ? accessToken.Value : null;
        }

        private Task OnStatusUpdated(Notification<StreamingStatus> status) =>
            InvokeAsync(() =>
            {
                _streamingStatus = status;
                StateHasChanged();
            });

        private Task OnTweetReceived(Notification<TweetContents> tweet) =>
            InvokeAsync(() =>
            {
                _ = _tweets.Add(tweet);
                StateHasChanged();
            });

        async ValueTask IAsyncDisposable.DisposeAsync()
        {
            if (_hubConnection is not null)
            {
                await _hubConnection.InvokeAsync("LeaveTweets");
                await _hubConnection.DisposeAsync();
            }
        }
    }
}
