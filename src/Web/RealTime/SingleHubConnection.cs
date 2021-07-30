// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Threading;
using System.Threading.Tasks;
using Learning.Blazor.Models;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Learning.Blazor
{
    public class SingleHubConnection
    {
        private readonly IAccessTokenProvider _tokenProvider = null!;
        private readonly IConfiguration _configuration = null!;
        private readonly HubConnection _hubConnection = null!;

        public HubConnectionState State => _hubConnection.State;

        public SingleHubConnection(
            IAccessTokenProvider tokenProvider,
            IConfiguration configuration)
        {
            (_tokenProvider, _configuration) = (tokenProvider, configuration);

            var notificationHub =
                new Uri($"{_configuration["WebApiServerUrl"]}/notifications");

            _hubConnection = new HubConnectionBuilder()
                .WithUrl(notificationHub,
                     options => options.AccessTokenProvider = GetAccessTokenValueAsync)
                .WithAutomaticReconnect()
                .AddMessagePackProtocol()
                .Build();
        }

        private async Task<string?> GetAccessTokenValueAsync()
        {
            var result = await _tokenProvider.RequestAccessToken();
            return result.TryGetToken(out var accessToken) ? accessToken.Value : null;
        }

        public Task StartAsync(CancellationToken token = default) =>
            State == HubConnectionState.Disconnected
                ? _hubConnection.StartAsync(token)
                : Task.CompletedTask;

        public Task StopAsync(CancellationToken token = default) =>
            State != HubConnectionState.Disconnected
                ? _hubConnection.StopAsync(token)
                : Task.CompletedTask;

        public Task JoinTweetsAsync() =>
            _hubConnection.InvokeAsync("JoinTweets");

        public Task LeaveTweetsAsync() =>
            _hubConnection.InvokeAsync("LeaveTweets");

        public Task StartTweetStreamAsync() =>
            _hubConnection.InvokeAsync("StartTweetStream");

        public IDisposable SubscribeToStatusUpdated(
            Func<Notification<StreamingStatus>, Task> onStatusUpdated) =>
            _hubConnection.On("StatusUpdated", onStatusUpdated);

        public IDisposable SubscribeToTweetReceived(
            Func<Notification<TweetContents>, Task> onTweetReceived) =>
            _hubConnection.On("TweetReceived", onTweetReceived);
    }
}
