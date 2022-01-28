// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor;

public sealed partial class SharedHubConnection : IAsyncDisposable
{
    private readonly Guid _id = Guid.NewGuid();
    private readonly IServiceProvider _serviceProvider = null!;
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
        IServiceProvider serviceProvider,
        IOptions<WebApiOptions> options,
        CultureService cultureService,
        ILogger<SharedHubConnection> logger)
    {
        (_serviceProvider, _cultureService, _logger) =
            (serviceProvider, cultureService, logger);

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

    Task OnHubConnectionClosedAsync(Exception? exception)
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

    Task OnHubConnectionReconnectingAsync(Exception? exception)
    {
        _logger.LogInformation(
            exception, "{Id}: Hub connection closed: {Exception}", _id, exception);

        return Task.CompletedTask;
    }
    
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
