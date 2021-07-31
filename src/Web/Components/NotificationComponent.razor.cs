// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Learning.Blazor.ComponentModels;
using Learning.Blazor.Models;
using Microsoft.AspNetCore.Components;

namespace Learning.Blazor.Components
{
    public sealed partial class NotificationComponent : IAsyncDisposable
    {
        private readonly Stack<IDisposable> _subscriptions = new();

        private List<NotificationComponentModel> _notifications = new();
        private bool _show = false;

        private string _showClass => _show ? "is-active" : "";

        [Inject]
        public SingleHubConnection HubConnection { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            _subscriptions.Push(
                HubConnection.SubscribeToUserLoggedIn(OnUserLoggedIn));
            _subscriptions.Push(
                HubConnection.SubscribeToUserLoggedOut(OnUserLoggedOut));

            await HubConnection.StartAsync(this);
        }

        private Task OnUserLoggedIn(Notification<Actor> notification) =>
            InvokeAsync(() =>
            {
                Actor actor = notification;
                var text = localize["UserLoggedInFormat", actor.UserName];

                _notifications.Add(new()
                {
                    Text = text,
                    IsDismissed = false,
                    NotificationType = notification.Type
                });
            });

        private Task OnUserLoggedOut(Notification<Actor> notification) =>
            InvokeAsync(() =>
            {
                Actor actor = notification;
                var text = localize["UserLoggedOutFormat", actor.UserName];

                _notifications.Add(new()
                {
                    Text = text,
                    IsDismissed = false,
                    NotificationType = notification.Type
                });
            });

        private void Show() => _show = true;

        private void Dismiss() => _show = false;

        private void DismissNotification(NotificationComponentModel notificationModel)
        {
            _notifications =
                _notifications.Select(
                    notification =>
                        notification == notificationModel
                            ? (notification with { IsDismissed = true }) : notification)
                    .ToList();
        }

        async ValueTask IAsyncDisposable.DisposeAsync()
        {
            if (HubConnection is not null)
            {
                await HubConnection.StopAsync(this);
            }

            while (_subscriptions.TryPop(out var disposable))
            {
                disposable.Dispose();
            }
        }
    }
}
