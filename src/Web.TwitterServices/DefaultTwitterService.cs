// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.TwitterServices;

internal sealed class DefaultTwitterService : ITwitterService
{
    private static readonly object s_locker = new();
    private static bool s_isInitialized = false;

    private readonly ILogger<DefaultTwitterService> _logger;
    private readonly TwitterClient _twitterClient;
    private readonly IFilteredStream _filteredStream;
    private readonly Stack<TweetContents> _latestTweets = new(50);
    private readonly HashSet<string> _tracks = new(StringComparer.OrdinalIgnoreCase)
    {
        "#Blazor",
        "#WebAssembly",
        "#wasm",
        "#csharp",
        "#dotnet",
        "@dotnet",
        "@aspnet",
        "@davidpine7"
    };

    private HashSet<long> _tweetIds = new();

    public IReadOnlyCollection<TweetContents>? LastFiftyTweets => _latestTweets;
    public StreamingStatus? CurrentStatus { get; private set; }

    public DefaultTwitterService(
        ILogger<DefaultTwitterService> logger,
        TwitterClient twitterClient,
        IFilteredStream filteredStream)
    {
        _logger = logger;
        _twitterClient = twitterClient;
        _filteredStream = filteredStream;

        InitializeStream();
    }

    public event Func<TweetContents, Task> TweetReceived = default!;
    public event Func<StreamingStatus, Task> StatusUpdated = default!;

    private void InitializeStream()
    {
        lock (s_locker)
        {
            if (s_isInitialized)
            {
                return;
            }

            _filteredStream.AddCustomQueryParameter("omit_script", "true");

            _filteredStream.DisconnectMessageReceived += OnDisconnectedMessageReceived;
            _filteredStream.MatchingTweetReceived += OnMatchingTweetReceived;
            _filteredStream.StreamStarted += OnStreamStarted;
            _filteredStream.StreamStopped += OnStreamStopped;
            _filteredStream.StreamResumed += OnStreamResumed;
            _filteredStream.StreamPaused += OnStreamPaused;
            _filteredStream.WarningFallingBehindDetected += OnFallingBehindDetected;

            foreach (var track in _tracks)
            {
                _filteredStream.AddTrack(track);
            }

            s_isInitialized = true;
        }
    }

    /// <inheritdoc />
    public async Task StartTweetStreamAsync()
    {
        try
        {
            if (_filteredStream is not { StreamState: StreamState.Running })
            {
                _logger.LogInformation("Starting tweet stream.");
                await _filteredStream.StartMatchingAnyConditionAsync();
                _logger.LogInformation("Started tweet stream.");
            }
            else
            {
                _logger.LogWarning("Unable to start tweet stream.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(
                "Unable to start tweet stream. {Error}", ex.Message);
        }
    }

    /// <inheritdoc />
    public async Task StopTweetStreamAsync()
    {
        try
        {
            if (_filteredStream is not { StreamState: StreamState.Stop })
            {
                _logger.LogInformation("Stopping tweet stream.");

                _filteredStream.Stop();
                await Task.CompletedTask;

                _logger.LogInformation("Stoppied tweet stream.");
            }
            else
            {
                _logger.LogWarning("Unable to stop tweet stream.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(
                "Unable to stop tweet stream. {Error}", ex.Message);
        }
    }

    private async void OnNonMatchingTweetReceived(
        object? sender, TweetEventArgs? args) =>
        await BroadcastTweetAsync(args?.Tweet, true);

    private async void OnMatchingTweetReceived(
        object? sender, MatchedTweetReceivedEventArgs? args) =>
        await BroadcastTweetAsync(args?.Tweet, false);

    private async Task BroadcastTweetAsync(ITweet? iTweet, bool isOffTopic)
    {
        if (iTweet is null)
        {
            _logger.LogError("Unable to broadcast tweet");
            return;
        }

        // If Twitter thinks this might be sensitive, filter it out.
        if (iTweet.PossiblySensitive)
        {
            _logger.LogWarning("Ignoring sensitive tweet: {Tweet}", iTweet.FullText);
            return;
        }

        GetOEmbedTweetParameters parameters = new(iTweet)
        {
            Alignment = OEmbedTweetAlignment.Center
        };
        var tweet = await _twitterClient.Tweets.GetOEmbedTweetAsync(parameters);
        if (tweet is null)
        {
            _logger.LogWarning("Unable to parse OEmbed tweet");
            return;
        }

        TweetContents latestTweet = new(
            iTweet.Id,
            isOffTopic,
            tweet.AuthorName,
            tweet.AuthorURL,
            tweet.HTML,
            tweet.URL,
            tweet.ProviderURL,
            tweet.Width,
            tweet.Height,
            tweet.Version,
            tweet.Type,
            tweet.CacheAge);

        lock (s_locker)
        {
            if (_tweetIds.Add(latestTweet.Id))
            {
                if (_latestTweets is { Count: > 0 })
                {
                    _ = _latestTweets.Pop();
                }
                _latestTweets.Push(latestTweet);
                _tweetIds = _latestTweets.Select(t => t.Id).ToHashSet();
            }
        }

        if (TweetReceived is not null)
        {
            _logger.LogInformation(
                "Successfully broadcasting tweet: {Tweet}", tweet.HTML);

            await TweetReceived(latestTweet);
        }
    }

    private async void OnDisconnectedMessageReceived(object? sender, DisconnectedEventArgs? args) =>
        await SendStatusUpdateAsync(
            "Twitter stream disconnected: {0}",
            args?.DisconnectMessage);

    private async void OnStreamStarted(object? sender, EventArgs? args) =>
        await SendStatusUpdateAsync("Twitter stream started.");

    private async void OnStreamStopped(object? sender, StreamStoppedEventArgs args) =>
        await SendStatusUpdateAsync(
            "Twitter stream stopped {0}: {1}",
            args.DisconnectMessage?.ToString() ?? "no disconnection reason",
            args.Exception?.Message ?? "no error (clean stop).");

    private async void OnStreamResumed(object? sender, EventArgs? args) =>
        await SendStatusUpdateAsync("Twitter stream resumed.");

    private async void OnStreamPaused(object? sender, EventArgs? args) =>
        await SendStatusUpdateAsync("Twitter stream paused.");

    private async void OnFallingBehindDetected(object? sender, WarningFallingBehindEventArgs args) =>
        await SendStatusUpdateAsync(
            "Twitter stream falling behind: {0}.",
            args.WarningMessage);

    private Task SendStatusUpdateAsync(
        string statusFormat, params object?[] args)
    {
        _logger.LogInformation(statusFormat, args);

        CurrentStatus = new(
            IsStreaming: _filteredStream.StreamState == StreamState.Running,
            Message: args is { Length: > 0 } ? string.Format(statusFormat, args) : statusFormat,
            Tracks: _tracks.ToArray());

        return StatusUpdated?.Invoke(CurrentStatus)
            ?? Task.CompletedTask;
    }
}
