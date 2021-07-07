// Copyright (c) 2021 David Pine. All rights reserved.
//  Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Learning.Blazor.Models;

namespace Learning.Blazor.TwitterServices
{
    public interface ITwitterService : IAsyncDisposable
    {
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
        /// Removes the track from listening on the Twitter stream.
        /// A track is a string that represents either a
        /// <c>#hashtag</c> or <c>@handle</c>.
        /// </summary>
        /// <param name="tracks">The tracks to add.</param>
        void RemoveTrack([NotNull] string track);

        /// <summary>
        /// Adds tracks to listen on from our Twitter stream.
        /// A track is a string that represents either:
        /// <c>#hashtag</c> or <c>@handle</c>.
        /// </summary>
        /// <param name="tracks">The tracks to add.</param>
        void AddTracks([NotNull] ISet<string> tracks);

        /// <summary>
        /// Starts the tweet stream, to pause the stream call
        /// <see cref="PauseTweetStream"/>, and to stop it use <see cref="StopTweetStream"/>.
        /// </summary>
        /// <returns></returns>
        Task StartTweetStreamAsync();

        /// <summary>
        /// Pauses the stream, to start it call <see cref="StartTweetStreamAsync"/>.
        /// </summary>
        void PauseTweetStream();

        /// <summary>
        /// Stops the stream, to start it call <see cref="StartTweetStreamAsync"/>.
        /// </summary>
        void StopTweetStream();
    }
}
