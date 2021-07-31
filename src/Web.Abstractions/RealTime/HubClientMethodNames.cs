// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System;

namespace Learning.Blazor.Abstractions.RealTime
{
    /// <summary>
    /// A class that contains method names that are invokeable
    /// from a SignalR connected client on the SignalR server's hub.
    /// </summary>
    public static class HubClientMethodNames
    {
        /// <summary>
        /// A method on the "/notification" endpoint, invokable from a hub connection that expects no parameters.
        /// The contextual users' connection will join the "Tweets" group.
        /// </summary>
        public const string JoinTweets = nameof(JoinTweets);

        /// <summary>
        /// A method on the "/notification" endpoint, invokable from a hub connection that expects no parameters.
        /// The contextual users' connection will leave the "Tweets" group.
        /// </summary>
        public const string LeaveTweets = nameof(LeaveTweets); // StartTweetStream

        /// <summary>
        /// A method on the "/notification" endpoint, invokable from a hub connection that expects no parameters.
        /// Starts the Tweet stream in the "Tweets" group.
        /// </summary>
        public const string StartTweetStream = nameof(StartTweetStream);

        /// <summary>
        /// A method on the "/notification" endpoint, invokable from a hub connection that expects a "room" name.
        /// The contextual users' connection will join the given "room" named group.
        /// </summary>
        public const string JoinChat = nameof(JoinChat);

        /// <summary>
        /// A method on the "/notification" endpoint, invokable from a hub connection that expects a "room" name.
        /// The contextual users' connection will leave the given "room" named group.
        /// </summary>
        public const string LeaveChat = nameof(LeaveChat);

        /// <summary>
        /// A method on the "/notification" endpoint, invokable from a hub connection expecting the following parameter(s):
        /// <list type="bullet">
        /// <item>A <see cref="string"/> named <c>room</c>, representing the room to post to.</item>
        /// <item>A <see cref="string"/> named <c>message</c>, representing the message to post.</item>
        /// <item>A nullable <see cref="Guid"/> named <c>id</c>, representing the message identifier.</item>
        /// </list>
        /// Posts or updates the given messsage to the given room.
        /// </summary>
        public const string PostOrUpdateMessage = nameof(PostOrUpdateMessage);

        /// <summary>
        /// A method on the "/notification" endpoint, invokable from a hub connection expecting the following parameter(s):
        /// <list type="bullet">
        /// <item>A <see cref="bool"/> isTyping <c>room</c>, representing whether the current user is typing.</item>
        /// </list>
        /// Toggles whether the user is typing.
        /// </summary>
        public const string ToggleUserTyping = nameof(ToggleUserTyping);
    }
}
