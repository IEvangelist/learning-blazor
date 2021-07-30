// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Learning.Blazor.Models;
using Microsoft.AspNetCore.Components;

namespace Learning.Blazor.Components
{
    public sealed partial class NotificationComponent : IAsyncDisposable
    {
        private readonly Stack<IDisposable> _subscriptions = new();

        [Inject]
        public SingleHubConnection HubConnection { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            _subscriptions.Push(HubConnection.SubscribeToUserLoggedIn(OnUserLoggedIn));
            _subscriptions.Push(HubConnection.SubscribeToUserLoggedOut(OnUserLoggedOut));

            await HubConnection.StartAsync();
        }

        private Task OnUserLoggedIn(Notification<Actor> arg) => throw new NotImplementedException();

        private Task OnUserLoggedOut(Notification<Actor> arg) => throw new NotImplementedException();

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
