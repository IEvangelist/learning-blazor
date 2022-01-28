// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Api.Services;

public sealed class TwitterWorkerService : BackgroundService
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
            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }

    private Task OnStatusUpdated(StreamingStatus status) =>
        _hubContext.Clients
            .Group(HubGroupNames.Tweets)
            .SendAsync(
                HubServerEventNames.StatusUpdated,
                Notification<StreamingStatus>.FromStatus(status));

    private Task OnTweetReceived(TweetContents tweet) =>
        _hubContext.Clients
            .Group(HubGroupNames.Tweets)
            .SendAsync(
                HubServerEventNames.TweetReceived,
                Notification<TweetContents>.FromTweet(tweet));
}
