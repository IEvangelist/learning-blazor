// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.TwitterServices;

public interface ITwitterService
{
    IReadOnlyCollection<TweetContents>? LastFiftyTweets { get; }
    StreamingStatus? CurrentStatus { get; }

    /// <summary>
    /// The event that is fired when a tweet arrives from the stream.
    /// Allows for async event handlers, who receive the <see cref="TweetContents"/>
    /// as they arrive in the stream (in real-time).
    /// </summary>
    event Func<TweetContents, Task> TweetReceived;

    /// <summary>
    /// The event that is fired when the streaming status has been updated.
    /// Allows for async event handlers, who receive the <see cref="StreamingStatus"/> updates.
    /// </summary>
    event Func<StreamingStatus, Task> StatusUpdated;

    /// <summary>
    /// Starts the tweet stream, to stop it use <see cref="StopTweetStreamAsync"/>.
    /// </summary>
    Task StartTweetStreamAsync();

    /// <summary>
    /// Stops the stream, to start it call <see cref="StartTweetStreamAsync"/>.
    /// </summary>
    Task StopTweetStreamAsync();
}
