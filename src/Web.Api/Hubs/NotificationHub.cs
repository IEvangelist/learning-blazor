// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
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

        public void RemoveTrack(string track) =>
            _twitterService.RemoveTrack(track);

        public void AddTracks(ISet<string> tracks) =>
            _twitterService.AddTracks(tracks);

        public Task StartStream() =>
            _twitterService.StartTweetStreamAsync();

        public void PauseStream() =>
            _twitterService.PauseTweetStream();

        public void StopStream() =>
            _twitterService.StopTweetStream();

        /* Additional notification hub functionality defined in TwitterWorkerService.cs */
    }
}
