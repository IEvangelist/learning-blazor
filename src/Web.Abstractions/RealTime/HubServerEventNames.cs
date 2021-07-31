// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using Learning.Blazor.Models;

namespace Learning.Blazor.Abstractions.RealTime
{
    /// <summary>
    /// A class that contains event names (and parameter details) that are fired from
    /// the SignalR server's hub that a SignalR connected client can subscribe to.
    /// </summary>
    public static class HubServerEventNames
    {
        /// <summary>
        /// Fires when a user logs into the app.
        /// Handlers should expect a <see cref="Notification{Actor}"/> where <c>T</c> is an <see cref="Actor"/>.
        /// </summary>
        public const string UserLoggedIn = nameof(UserLoggedIn);

        /// <summary>
        /// Fires when a user logs out of the app.
        /// Handlers should expect a <see cref="Notification{Actor}"/> where <c>T</c> is an <see cref="Actor"/>.
        /// </summary>
        public const string UserLoggedOut = nameof(UserLoggedOut);

        /// <summary>
        /// Fires when a chat user typing state changes.
        /// Handlers should expect a <see cref="Notification{ActorAction}"/> where <c>T</c> is an <see cref="ActorAction"/>.
        /// </summary>
        public const string UserTyping = nameof(UserTyping);

        /// <summary>
        /// Fires when a chat user sends or updates a message.
        /// Handlers should expect a <see cref="Notification{ActorMessage}"/> where <c>T</c> is an <see cref="ActorMessage"/>.
        /// </summary>
        public const string MessageReceived = nameof(MessageReceived);

        /// <summary>
        /// Fires when the twitter streaming status changes.
        /// Handlers should expect a <see cref="Notification{StreamingStatus}"/> where <c>T</c> is an <see cref="StreamingStatus"/>.
        /// </summary>
        public const string StatusUpdated = nameof(StatusUpdated);

        /// <summary>
        /// Fires when a tweet is received.
        /// Handlers should expect a <see cref="Notification{TweetContents}"/> where <c>T</c> is an <see cref="TweetContents"/>.
        /// </summary>
        public const string TweetReceived = nameof(TweetReceived);
    }
}
