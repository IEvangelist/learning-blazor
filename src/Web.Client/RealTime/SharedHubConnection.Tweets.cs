// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor;

public sealed partial class SharedHubConnection
{
    /// <inheritdoc cref="HubClientMethodNames.JoinTweets" />
    public Task JoinTweetsAsync() =>
        _hubConnection.InvokeAsync(
            methodName: HubClientMethodNames.JoinTweets);

    /// <inheritdoc cref="HubClientMethodNames.LeaveTweets" />
    public Task LeaveTweetsAsync() =>
        _hubConnection.InvokeAsync(
            methodName: HubClientMethodNames.LeaveTweets);

    /// <inheritdoc cref="HubClientMethodNames.StartTweetStream" />
    public Task StartTweetStreamAsync() =>
        _hubConnection.InvokeAsync(
            methodName: HubClientMethodNames.StartTweetStream);

    /// <inheritdoc cref="HubServerEventNames.StatusUpdated" />
    public IDisposable SubscribeToStatusUpdated(
        Func<Notification<StreamingStatus>, Task> onStatusUpdated) =>
        _hubConnection.On(
            methodName: HubServerEventNames.StatusUpdated,
            handler: onStatusUpdated);

    /// <inheritdoc cref="HubServerEventNames.TweetReceived" />
    public IDisposable SubscribeToTweetReceived(
        Func<Notification<TweetContents>, Task> onTweetReceived) =>
        _hubConnection.On(
            methodName: HubServerEventNames.TweetReceived,
            handler: onTweetReceived);

    /// <inheritdoc cref="HubServerEventNames.InitialTweetsLoaded" />
    public IDisposable SubscribeToTweetsLoaded(
        Func<Notification<List<TweetContents>>, Task> onTweetsLoaded) =>
        _hubConnection.On(
            methodName: HubServerEventNames.InitialTweetsLoaded,
            handler: onTweetsLoaded);

}
