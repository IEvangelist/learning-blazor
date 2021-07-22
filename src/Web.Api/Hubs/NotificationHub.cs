// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System.Threading.Tasks;
using Learning.Blazor.TwitterServices;
using Microsoft.AspNetCore.SignalR;

namespace Learning.Blazor.Api.Hubs
{
    public class NotificationHub : Hub
    {
        private readonly ITwitterService _twitterService;

        public NotificationHub(ITwitterService twitterService) =>
            _twitterService = twitterService;

        public Task JoinTweets() =>
            Groups.AddToGroupAsync(Context.ConnectionId, "Tweets");

        public Task LeaveTweets() =>
            Groups.RemoveFromGroupAsync(Context.ConnectionId, "Tweets");

        public Task StartTweetStream() =>
            _twitterService.StartTweetStreamAsync();

        /* Additional notification hub functionality 
         * defined in TwitterWorkerService.cs:
         *      TweetReceived
         *      StatusUpdated
         */
    }
}
