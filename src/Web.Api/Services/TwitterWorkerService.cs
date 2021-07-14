// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using Learning.Blazor.Api.Hubs;
using Learning.Blazor.Models;
using Learning.Blazor.TwitterServices;
using Microsoft.AspNetCore.SignalR;

namespace Learning.Blazor.Api.Services
{
    public class TwitterWorkerService : BackgroundService, IAsyncDisposable
    {
        private readonly ITwitterService _twitterService;
        private readonly IHubContext<NotificationHub> _hubContext;

        public TwitterWorkerService(
            ITwitterService twitterService,
            IHubContext<NotificationHub> hubContext)
        {
            (_twitterService, _hubContext) = (twitterService, hubContext);

            _twitterService.StatusUpdated += OnStatusUpdated;
            _twitterService.TweetReceived += OnTweetReceived;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                stoppingToken.ThrowIfCancellationRequested();

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }

        private Task OnStatusUpdated(StreamingStatus status) =>
            _hubContext.Clients.All.SendAsync(
                "StatusUpdated", Notification<StreamingStatus>.FromStatus(status));

        private Task OnTweetReceived(TweetContents tweet) =>
            _hubContext.Clients.All.SendAsync(
                "TweetReceived", Notification<TweetContents>.FromTweet(tweet));

        ValueTask IAsyncDisposable.DisposeAsync() => _twitterService.DisposeAsync();
    }
}
