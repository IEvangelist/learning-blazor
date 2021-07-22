// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Learning.Blazor.Extensions;
using Learning.Blazor.Models;
using Microsoft.Extensions.Logging;
using Tweetinvi.Events;
using Tweetinvi.Models;
using Tweetinvi.Streaming;

namespace Learning.Blazor.TwitterServices
{
    internal sealed class DefaultTwitterService : ITwitterService
    {
        private static readonly object s_locker = new();
        private static bool s_isInitialized = false;

        private readonly ILogger<DefaultTwitterService> _logger;
        private readonly IFilteredStream _filteredStream;

        public DefaultTwitterService(
            ILogger<DefaultTwitterService> logger,
            IFilteredStream filteredStream)
        {
            _logger = logger;
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

                // The script is loaded only once, registered in our twitter-componenet.js
                // No need to have each individual tweet have the script embedded within it.
                _filteredStream.AddCustomQueryParameter("omit_script", "true");

                _filteredStream.DisconnectMessageReceived += OnDisconnectedMessageReceived;
                _filteredStream.MatchingTweetReceived += OnMatchingTweetReceived;
                _filteredStream.StreamStarted += OnStreamStarted;
                _filteredStream.StreamStopped += OnStreamStopped;
                _filteredStream.StreamResumed += OnStreamResumed;
                _filteredStream.StreamPaused += OnStreamPaused;
                _filteredStream.WarningFallingBehindDetected += OnFallingBehindDetected;

                AddTracks(new HashSet<string>(StringComparer.OrdinalIgnoreCase)
                {
                    "#Blazor",
                    "#WebAssembly",
                    "#wasm",
                    "#csharp",
                    "#dotnet",
                    "@dotnet",
                    "@aspnet",
                    "@davidpine7"
                });

                s_isInitialized = true;
            }
        }

        /// <inheritdoc />
        public void AddTracks([NotNull] ISet<string> tracks) =>
            HandleTracks(true, tracks.ToArray());

        /// <inheritdoc />
        public void RemoveTrack([NotNull] string track) =>
            HandleTracks(false, track);

        private void HandleTracks(bool add, params string[] tracks)
        {
            StopTweetStream();

            foreach (var track in tracks.Where(_ => _ is not null))
            {
                if (add)
                {
                    _filteredStream.AddTrack(track);
                }
                else
                {
                    _filteredStream.RemoveTrack(track);
                }
            }
        }

        /// <inheritdoc />
        public void PauseTweetStream()
        {
            if (_filteredStream is not { StreamState: StreamState.Pause })
            {
                _logger.LogInformation("Pausing tweet stream.");
                _filteredStream.Pause();
                _logger.LogInformation("Paused tweet stream.");
            }
            else
            {
                _logger.LogWarning("Unable to pause tweet stream.");
            }
        }

        /// <inheritdoc />
        public async Task StartTweetStreamAsync()
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

        /// <inheritdoc />
        public void StopTweetStream()
        {
            if (_filteredStream is not { StreamState: StreamState.Stop })
            {
                _logger.LogInformation("Stopping tweet stream.");
                _filteredStream.Stop();
                _logger.LogInformation("Stoppied tweet stream.");
            }
            else
            {
                _logger.LogWarning("Unable to stop tweet stream.");
            }
        }

        ValueTask IAsyncDisposable.DisposeAsync() =>
            _filteredStream.TryDisposeAsync();

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

            var tweet = await iTweet.GenerateOEmbedTweetAsync();
            if (tweet is null)
            {
                _logger.LogWarning("Unable to parse OEmbed tweet");
                return;
            }

            if (TweetReceived is not null)
            {
                _logger.LogWarning("Successfully broadcasting tweet: {Tweet}", tweet.HTML);

                await TweetReceived(
                    new TweetContents
                    {
                        IsOffTopic = isOffTopic,
                        AuthorName = tweet.AuthorName,
                        AuthorURL = tweet.AuthorURL,
                        CacheAge = tweet.CacheAge,
                        Height = tweet.Height,
                        HTML = tweet.HTML,
                        ProviderURL = tweet.ProviderURL,
                        Type = tweet.Type,
                        URL = tweet.URL,
                        Version = tweet.Version,
                        Width = tweet.Width
                    });
            }
        }

        private async void OnDisconnectedMessageReceived(object? sender, DisconnectedEventArgs? args)
        {
            var status = $"Twitter stream disconnected: {args?.DisconnectMessage}";

            _logger.LogWarning(status, args);

            await SendStatusUpdateAsync(status);
        }

        private async void OnStreamStarted(object? sender, EventArgs? args)
        {
            const string status = "Twitter stream started.";

            _logger.LogInformation(status);

            await SendStatusUpdateAsync(status);
        }

        private async void OnStreamStopped(object? sender, StreamStoppedEventArgs args)
        {
            var disconnectMessage = args.DisconnectMessage?.ToString() ?? "no disconnection reason";
            var errorMessage = args.Exception?.Message ?? "no error (clean stop).";
            var status = $"Twitter stream stopped {disconnectMessage}: {errorMessage}";

            _logger.LogInformation(status);

            await SendStatusUpdateAsync(status);
        }

        private async void OnStreamResumed(object? sender, EventArgs? args)
        {
            const string status = "Twitter stream resumed.";

            _logger.LogInformation(status);

            await SendStatusUpdateAsync(status);
        }

        private async void OnStreamPaused(object? sender, EventArgs? args)
        {
            const string status = "Twitter stream paused.";

            _logger.LogInformation(status);

            await SendStatusUpdateAsync(status);
        }

        private async void OnFallingBehindDetected(object? sender, WarningFallingBehindEventArgs args)
        {
            var status = $"Twitter stream falling behind: {args.WarningMessage}.";

            _logger.LogInformation(status);

            await SendStatusUpdateAsync(status);
        }

        private Task SendStatusUpdateAsync(string status)
        {
            _logger.LogInformation(status);

            StreamingStatus streamingStatus = new(
                    IsStreaming: _filteredStream.StreamState == StreamState.Running,
                    Message: status);

            return StatusUpdated?.Invoke(streamingStatus)
                ?? Task.CompletedTask;
        }
    }
}
