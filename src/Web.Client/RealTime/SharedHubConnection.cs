// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor;

public sealed class SharedHubConnection : IAsyncDisposable
{
    private readonly Guid _id = Guid.NewGuid();
    private readonly IAccessTokenProvider _tokenProvider = null!;
    private readonly ILogger<SharedHubConnection> _logger = null!;
    private readonly AsyncRetryPolicy _asyncRetryPolicy = null!;
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
        IOptions<WebApiOptions> options,
        CultureService cultureService,
        ILogger<SharedHubConnection> logger)
    {
        (_tokenProvider, _cultureService, _logger) =
            (tokenProvider, cultureService, logger);

        var notificationHub =
            new Uri($"{options.Value.WebApiServerUrl}/notifications");

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

        _asyncRetryPolicy = Policy.Handle<Exception>()
            .WaitAndRetryForeverAsync(
                attempt => TimeSpan.FromMilliseconds(5_000),
                (ex, calculatedDuration) => _logger.LogError(ex.Message, ex));
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

                await _asyncRetryPolicy.ExecuteAsync(
                    async cancellationToken =>
                        await _hubConnection.StartAsync(cancellationToken), token) ;
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

    /// <inheritdoc cref="HubClientMethodNames.JoinTweets" />
    public Task JoinTweetsAsync() =>
        _hubConnection.InvokeAsync(HubClientMethodNames.JoinTweets);

    /// <inheritdoc cref="HubClientMethodNames.LeaveTweets" />
    public Task LeaveTweetsAsync() =>
        _hubConnection.InvokeAsync(HubClientMethodNames.LeaveTweets);

    /// <inheritdoc cref="HubClientMethodNames.StartTweetStream" />
    public Task StartTweetStreamAsync() =>
        _hubConnection.InvokeAsync(HubClientMethodNames.StartTweetStream);

    /// <inheritdoc cref="HubClientMethodNames.JoinChat" />
    public Task JoinChatAsync(string room) =>
        _hubConnection.InvokeAsync(HubClientMethodNames.JoinChat, room);

    /// <inheritdoc cref="HubClientMethodNames.LeaveChat" />
    public Task LeaveChatAsync(string room) =>
        _hubConnection.InvokeAsync(HubClientMethodNames.LeaveChat, room);

    /// <inheritdoc cref="HubClientMethodNames.PostOrUpdateMessage" />
    public Task PostOrUpdateMessageAsync(string room, string message, Guid? id = default) =>
        _hubConnection.InvokeAsync(HubClientMethodNames.PostOrUpdateMessage, room, message, id);

    /// <inheritdoc cref="HubClientMethodNames.ToggleUserTyping" />
    public Task ToggleUserTypingAsync(bool isTyping) =>
        _hubConnection.InvokeAsync(HubClientMethodNames.ToggleUserTyping, isTyping);

    /// <inheritdoc cref="HubServerEventNames.StatusUpdated" />
    public IDisposable SubscribeToStatusUpdated(
        Func<Notification<StreamingStatus>, Task> onStatusUpdated) =>
        _hubConnection.On(HubServerEventNames.StatusUpdated, onStatusUpdated);

    /// <inheritdoc cref="HubServerEventNames.TweetReceived" />
    public IDisposable SubscribeToTweetReceived(
        Func<Notification<TweetContents>, Task> onTweetReceived) =>
        _hubConnection.On(HubServerEventNames.TweetReceived, onTweetReceived);

    /// <inheritdoc cref="HubServerEventNames.InitialTweetsLoaded" />
    public IDisposable SubscribeToTweetsLoaded(
        Func<Notification<HashSet<TweetContents>>, Task> onTweetsLoaded) =>
        _hubConnection.On(HubServerEventNames.InitialTweetsLoaded, onTweetsLoaded);

    /// <inheritdoc cref="HubServerEventNames.UserLoggedIn" />
    public IDisposable SubscribeToUserLoggedIn(
        Func<Notification<Actor>, Task> onUserLoggedIn) =>
        _hubConnection.On(HubServerEventNames.UserLoggedIn, onUserLoggedIn);

    /// <inheritdoc cref="HubServerEventNames.UserLoggedOut" />
    public IDisposable SubscribeToUserLoggedOut(
        Func<Notification<Actor>, Task> onUserLoggedOut) =>
        _hubConnection.On(HubServerEventNames.UserLoggedOut, onUserLoggedOut);

    /// <inheritdoc cref="HubServerEventNames.UserTyping" />
    public IDisposable SubscribeToUserTyping(
        Func<Notification<ActorAction>, Task> onUserTyping) =>
        _hubConnection.On(HubServerEventNames.UserTyping, onUserTyping);

    /// <inheritdoc cref="HubServerEventNames.MessageReceived" />
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
