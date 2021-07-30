// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Learning.Blazor.Components
{
    public partial class NotificationComponent : IAsyncDisposable
    {
        private readonly Stack<IDisposable> _subscriptions = new();

        [Inject]
        public SingleHubConnection HubConnection { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            _subscriptions.Push(HubConnection.SubscribeToStatusUpdated(OnStatusUpdated));
            _subscriptions.Push(HubConnection.SubscribeToTweetReceived(OnTweetReceived));

            await HubConnection.StartAsync();
            await HubConnection.JoinTweetsAsync();
            await HubConnection.StartTweetStreamAsync();
        }

        async ValueTask IAsyncDisposable.DisposeAsync()
        {
            if (HubConnection is not null)
            {
                await HubConnection.LeaveTweetsAsync();
            }

            while (_subscriptions.TryPop(out var disposable))
            {
                disposable.Dispose();
            }
        }
    }
}
