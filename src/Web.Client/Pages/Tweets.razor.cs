// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Pages
{
    public sealed partial class Tweets : IAsyncDisposable
    {
        private readonly ConcurrentDictionary<long, TweetContents> _tweets = new();
        private readonly Stack<IDisposable> _subscriptions = new();

        private StreamingStatus? _streamingStatus;
        private string? _filter = null!;

        private string _streamingFontAwesomeClass =>
            HubConnection is { State: HubConnectionState.Connected } &&
            _streamingStatus is { IsStreaming: true }
                ? "fas fa-link has-text-primary"
                : "fas fa-unlink has-text-warning";

        [Inject]
        public SharedHubConnection HubConnection { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            _subscriptions.Push(
                HubConnection.SubscribeToStatusUpdated(OnStatusUpdatedAsync));
            _subscriptions.Push(
                HubConnection.SubscribeToTweetReceived(OnTweetReceivedAsync));
            _subscriptions.Push(
                HubConnection.SubscribeToTweetsLoaded(OnTweetsLoadedAsync));

            await HubConnection.StartAsync();
            await HubConnection.JoinTweetsAsync();
            await HubConnection.StartTweetStreamAsync();
        }

        private Task OnStatusUpdatedAsync(Notification<StreamingStatus> status) =>
            InvokeAsync(() =>
            {
                if (_streamingStatus?.IsStreaming != status.Payload.IsStreaming)
                {
                    _streamingStatus = status;
                    StateHasChanged();
                }
            });

        private Task OnTweetReceivedAsync(Notification<TweetContents> tweet) =>
            InvokeAsync(() =>
            {
                if (!_tweets.ContainsKey(tweet.Payload.Id))
                {
                    _tweets[tweet.Payload.Id] = tweet;
                    StateHasChanged();
                }
            });

        private Task OnTweetsLoadedAsync(Notification<List<TweetContents>> tweets) =>
            InvokeAsync(() =>
            {
                var stateChanged = false;
                foreach (var tweet in tweets.Payload)
                {
                    if (!_tweets.ContainsKey(tweet.Id))
                    {
                        _tweets[tweet.Id] = tweet;
                        stateChanged = true;
                    }
                }

                if (stateChanged)
                {
                    StateHasChanged();
                }
            });

        async ValueTask IAsyncDisposable.DisposeAsync()
        {
            if (Logger.IsEnabled(LogLevel.Information))
            {
                Logger.LogInformation("Disposing of Tweets.razor.cs");
            }

            if (HubConnection is not null)
            {
                await HubConnection.LeaveTweetsAsync();
            }

            while (_subscriptions.TryPop(out var disposable))
            {
                disposable.Dispose();
            }
        }
    }
}
