// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System.Threading.Tasks;
using Learning.Blazor.Models;
using Learning.Blazor.TwitterServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Identity.Web.Resource;

namespace Learning.Blazor.Api.Hubs
{
    [Authorize, RequiredScope(new[] { "User.ApiAccess" })]
    public class NotificationHub : Hub
    {
        private readonly ITwitterService _twitterService;

        public NotificationHub(ITwitterService twitterService) =>
            _twitterService = twitterService;

        public async Task JoinTweets()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "Tweets");

            if (_twitterService.CurrentStatus is not null)
            {
                await Clients.Caller.SendAsync(
                    "StatusUpdated",
                    Notification<StreamingStatus>.FromStatus(_twitterService.CurrentStatus));
            }
            if (_twitterService.LastThreeTweets is { Count: > 0 })
            {
                foreach (var tweet in _twitterService.LastThreeTweets)
                {
                    await Clients.Caller.SendAsync(
                        "TweetReceived",
                        Notification<TweetContents>.FromTweet(tweet));
                }
            }
        }

        public Task LeaveTweets() =>
            Groups.RemoveFromGroupAsync(Context.ConnectionId, "Tweets");

        public Task StartTweetStream() =>
            _twitterService.StartTweetStreamAsync();

        public Task JoinChat(string room) =>
            Groups.AddToGroupAsync(Context.ConnectionId, room);

        public Task LeaveChat(string room) =>
            Groups.RemoveFromGroupAsync(Context.ConnectionId, room);

        /* Additional notification hub functionality 
         * defined in TwitterWorkerService.cs:
         *      TweetReceived
         *      StatusUpdated
         */
    }
}
