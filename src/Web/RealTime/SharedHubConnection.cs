// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Learning.Blazor.Abstractions.RealTime;
using Learning.Blazor.Models;
using Learning.Blazor.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Learning.Blazor
{
    public sealed class SharedHubConnection : IAsyncDisposable
    {
        private readonly Guid _id = Guid.NewGuid();
        private readonly IAccessTokenProvider _tokenProvider = null!;
        private readonly IConfiguration _configuration = null!;
        private readonly ILogger<SharedHubConnection> _logger = null!;
        private readonly CultureService _cultureService = null!;
        private readonly HubConnection _hubConnection = null!;
        private readonly SemaphoreSlim _lock = new(1, 1);
        private readonly HashSet<ComponentBase> _activeComponents = new();

        /// <summary>
        /// Indicates the state of the <see cref="HubConnection"/> to the server.
        /// </summary>
        public HubConnectionState State => _hubConnection.State;

        public SharedHubConnection(
            IAccessTokenProvider tokenProvider,
            IConfiguration configuration,
            CultureService cultureService,
            ILogger<SharedHubConnection> logger)
        {
            (_tokenProvider, _configuration, _cultureService, _logger) =
                (tokenProvider, configuration, cultureService, logger);

            var notificationHub =
                new Uri($"{_configuration["WebApiServerUrl"]}/notifications");

            _hubConnection = new HubConnectionBuilder()
                .WithUrl(notificationHub,
                     options =>
                     {
                         options.AccessTokenProvider = GetAccessTokenValueAsync;
                         options.Headers.Add(
                             "Accept-Language",
                             _cultureService.CurrentCulture.TwoLetterISOLanguageName);
                     })
                .WithAutomaticReconnect()
                .AddMessagePackProtocol()
                .Build();

            _hubConnection.Closed += OnHubConnectionClosedAsync;
            _hubConnection.Reconnected += OnHubConnectionReconnectedAsync;
            _hubConnection.Reconnecting += OnHubConnectionReconnectingAsync;
        }

        private Task OnHubConnectionClosedAsync(Exception? exception)
        {
            _logger.LogInformation(
                exception, "{Id}: Hub connection closed: {Exception}", _id, exception);

            return Task.CompletedTask;
        }

        private Task OnHubConnectionReconnectedAsync(string? message)
        {
            _logger.LogInformation(
                "{Id}: Hub connection reconnected: {Message}", _id, message);

            return Task.CompletedTask;
        }

        private Task OnHubConnectionReconnectingAsync(Exception? exception)
        {
            _logger.LogInformation(
                exception, "{Id}: Hub connection closed: {Exception}", _id, exception);

            return Task.CompletedTask;
        }

        private async Task<string?> GetAccessTokenValueAsync()
        {
            var result = await _tokenProvider.RequestAccessToken();
            if (result.TryGetToken(out var accessToken))
            {
                return accessToken.Value;
            }

            _logger.LogWarning(
                "Unable to get the access token. '{Status}' - Return URL: {Url}",
                result.Status, result.RedirectUrl);

            return null;
        }

        public async Task StartAsync(ComponentBase component, CancellationToken token = default)
        {
            _logger.LogDebug("{Id}: {Type} is acquiring start lock.", _id, component.GetType());
            await _lock.WaitAsync(token);

            try
            {
                _ = _activeComponents.Add(component);
                if (_activeComponents.Count > 0 && State == HubConnectionState.Disconnected)
                {
                    _logger.LogDebug("{Id}: {Type} is starting hub connection.", _id, component.GetType());
                    await _hubConnection.StartAsync(token);
                }
                else
                {
                    _logger.LogDebug(
                        "{Id}: {Type} requested start, unable to start. " +
                        "Active component count: {Count}, and connection state: {State}",
                        _id, component.GetType(),
                        _activeComponents.Count,
                        State);
                }
            }
            finally
            {
                _logger.LogDebug("{Id}: {Type} is releasing start lock.", _id, component.GetType());
                _lock.Release();
            }
        }

        public async Task StopAsync(ComponentBase component, CancellationToken token = default)
        {
            _logger.LogDebug("{Id}: {Type} is acquiring stop lock.", _id, component.GetType());
            await _lock.WaitAsync(token);

            try
            {
                _ = _activeComponents.Remove(component);
                if (_activeComponents.Count == 0 && State != HubConnectionState.Disconnected)
                {
                    _logger.LogDebug("{Id}: {Type} is stopping hub connection.", _id, component.GetType());
                    await _hubConnection.StopAsync(token);
                }
                else
                {
                    _logger.LogDebug(
                        "{Id}: {Type} requested stop, unable to stop. " +
                        "Active component count: {Count}, and connection state: {State}",
                        _id, component.GetType(),
                        _activeComponents.Count,
                        State);
                }
            }
            finally
            {
                _logger.LogDebug("{Id}: {Type} is releasing stop lock.", _id, component.GetType());
                _lock.Release();
            }
        }

        public Task JoinTweetsAsync() =>
            _hubConnection.InvokeAsync(HubClientMethodNames.JoinTweets);

        public Task LeaveTweetsAsync() =>
            _hubConnection.InvokeAsync(HubClientMethodNames.LeaveTweets);

        public Task StartTweetStreamAsync() =>
            _hubConnection.InvokeAsync(HubClientMethodNames.StartTweetStream);

        public Task JoinChatAsync(string room) =>
            _hubConnection.InvokeAsync(HubClientMethodNames.JoinChat, room);

        public Task LeaveChatAsync(string room) =>
            _hubConnection.InvokeAsync(HubClientMethodNames.LeaveChat, room);

        public Task PostOrUpdateMessageAsync(string room, string message, Guid? id = default) =>
            _hubConnection.InvokeAsync(HubClientMethodNames.PostOrUpdateMessage, room, message, id);

        public Task ToggleUserTypingAsync(bool isTyping) =>
            _hubConnection.InvokeAsync(HubClientMethodNames.ToggleUserTyping, isTyping);

        public IDisposable SubscribeToStatusUpdated(
            Func<Notification<StreamingStatus>, Task> onStatusUpdated) =>
            _hubConnection.On(HubServerEventNames.StatusUpdated, onStatusUpdated);

        public IDisposable SubscribeToTweetReceived(
            Func<Notification<TweetContents>, Task> onTweetReceived) =>
            _hubConnection.On(HubServerEventNames.TweetReceived, onTweetReceived);

        public IDisposable SubscribeToUserLoggedIn(
            Func<Notification<Actor>, Task> onUserLoggedIn) =>
            _hubConnection.On(HubServerEventNames.UserLoggedIn, onUserLoggedIn);

        public IDisposable SubscribeToUserLoggedOut(
            Func<Notification<Actor>, Task> onUserLoggedOut) =>
            _hubConnection.On(HubServerEventNames.UserLoggedOut, onUserLoggedOut);

        public IDisposable SubscribeToUserTyping(
            Func<Notification<ActorAction>, Task> onUserTyping) =>
            _hubConnection.On(HubServerEventNames.UserTyping, onUserTyping);

        public IDisposable SubscribeToMessageReceived(
            Func<Notification<ActorMessage>, Task> onMessageReceived) =>
            _hubConnection.On(HubServerEventNames.MessageReceived, onMessageReceived);

        async ValueTask IAsyncDisposable.DisposeAsync()
        {
            if (_hubConnection is not null)
            {
                _hubConnection.Closed -= OnHubConnectionClosedAsync;
                _hubConnection.Reconnected -= OnHubConnectionReconnectedAsync;
                _hubConnection.Reconnecting -= OnHubConnectionReconnectingAsync;

                await _hubConnection.StopAsync();
                await _hubConnection.DisposeAsync();
            }

            if (_lock is not null)
            {
                _lock.Dispose();
            }
        }
    }
}
