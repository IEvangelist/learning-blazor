// Copyright (c) 2021 David Pine. All rights reserved.
//  Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Learning.Blazor.Models;
using Microsoft.Extensions.Logging;
using Tweetinvi.Events;
using Tweetinvi.Models;
using Tweetinvi.Streaming;

namespace Learning.Blazor.Twitter.Services
{
    internal sealed class DefaultTwitterService : ITwitterService
    {
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
            _filteredStream.AddCustomQueryParameter("omit_script", "true");

            //_filteredStream.KeepAliveReceived += async (o, e) =>
            //    await SendStatusUpdateAsync("Keep alive recieved...");
            //_filteredStream.LimitReached += async (o, e) =>
            //    await SendStatusUpdateAsync($"Limit receached, missed {e.NumberOfTweetsNotReceived:#,#} tweets...");
            //_filteredStream.JsonObjectReceived += async (o, e) =>
            //    await SendStatusUpdateAsync($"JSON recieved {e.Json}...");
            //_filteredStream.UnmanagedEventReceived += async (o, e) =>
            //    await SendStatusUpdateAsync($"Unexpected JSON message recieved {e.JsonMessageReceived}...");

            _filteredStream.DisconnectMessageReceived += OnDisconnectedMessageReceived;
            _filteredStream.MatchingTweetReceived += OnMatchingTweetReceived;
            _filteredStream.StreamStarted += OnStreamStarted;
            _filteredStream.StreamStopped += OnStreamStopped;
            _filteredStream.StreamResumed += OnStreamResumed;
            _filteredStream.StreamPaused += OnStreamPaused;
            _filteredStream.WarningFallingBehindDetected += OnFallingBehindDetected;
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

            foreach (string? track in tracks.Where(_ => _ is not null))
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
            }
        }

        /// <inheritdoc />
        public async Task StartTweetStreamAsync()
        {
            if (_filteredStream is not { StreamState: StreamState.Running })
            {
                _logger.LogInformation("Starting tweet stream.");
                await _filteredStream.StartMatchingAllConditionsAsync();
            }
        }

        /// <inheritdoc />
        public void StopTweetStream()
        {
            if (_filteredStream is not { StreamState: StreamState.Stop })
            {
                _logger.LogInformation("Stopping tweet stream.");
                _filteredStream.Stop();
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (_filteredStream is IAsyncDisposable asyncDisposable)
            {
                await asyncDisposable.ConfigureAwait(false).DisposeAsync();
            }
            else if (_filteredStream is IDisposable disposable)
            {
                disposable.Dispose();
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
                return;
            }

            // If Twitter thinks this might be sensitive, filter it out.
            if (iTweet.PossiblySensitive)
            {
                return;
            }

            IOEmbedTweet? tweet = await iTweet.GenerateOEmbedTweetAsync();
            if (tweet is null)
            {
                return;
            }

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

        private Task SendStatusUpdateAsync(string status) =>
            StatusUpdated?.Invoke(
                new StreamingStatus(
                    IsStreaming: _filteredStream.StreamState == StreamState.Running,
                    Message: status))
                ?? Task.CompletedTask;
    }
}
