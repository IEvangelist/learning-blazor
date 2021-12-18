﻿// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Components
{
    public sealed partial class NotificationComponent : IAsyncDisposable
    {
        private readonly Stack<IDisposable> _subscriptions = new();

        private HashSet<NotificationComponentModel> _notifications = new();
        private bool _show = false;
        private DateTime? _latestNotificationDateTime = null!;


        private string _showClass => _show ? "is-active" : "";

        [Inject]
        public IHttpClientFactory HttpFactory { get; set; } = null!;

        [Inject]
        public SharedHubConnection HubConnection { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            _subscriptions.Push(
                HubConnection.SubscribeToUserLoggedIn(OnUserLoggedIn));
            _subscriptions.Push(
                HubConnection.SubscribeToUserLoggedOut(OnUserLoggedOut));

            await HubConnection.StartAsync(this);
        }

        private Task OnUserLoggedIn(Notification<Actor> notification) =>
            InvokeAsync(async () =>
            {
                var emails = User?.GetEmailAddresses();

                Actor actor = notification;
                var intersectingEmails = actor.Emails?.Intersect(emails ?? Array.Empty<string>())?.ToList();
                if (intersectingEmails is { Count: > 0 })
                {
                    var httpClient = HttpFactory.CreateClient(HttpClientNames.PwnedServerApi);
                    foreach (var email in intersectingEmails)
                    {
                        var breaches = (await httpClient.GetFromJsonAsync<BreachHeader[]>(
                            $"api/pwned/breaches/{email}",
                            BreachHeadersJsonSerializerContext.DefaultTypeInfo))
                            ?? Array.Empty<BreachHeader>();
                        if (breaches is { Length: > 0 })
                        {
                            var url = Navigation.ToAbsoluteUri($"/pwned/breaches?email={email}");
                            var link = $"<a href='{url}'><i class='fas fa-exclamation-circle'></i></a>";

                            _ = _notifications.Add(new()
                            {
                                Text = Localizer["EmailFoundInBreachFormat", email, breaches.Length, link],
                                NotificationType = NotificationType.Alert
                            });
                        }
                    }
                }
                else
                {
                    var text = Localizer["UserLoggedInFormat", actor.UserName];
                    _ = _notifications.Add(new()
                    {
                        Text = text,
                        NotificationType = notification.Type
                    });
                }

                _latestNotificationDateTime = DateTime.Now;

                StateHasChanged();
            });

        private Task OnUserLoggedOut(Notification<Actor> notification) =>
            InvokeAsync(() =>
            {
                Actor actor = notification;
                var text = Localizer["UserLoggedOutFormat", actor.UserName];

                _ = _notifications.Add(new()
                {
                    Text = text,
                    IsDismissed = false,
                    NotificationType = notification.Type
                });

                _latestNotificationDateTime = DateTime.Now;

                StateHasChanged();
            });

        private void Show() => _show = true;

        private void Dismiss() => _show = false;

        private void DismissNotification(NotificationComponentModel notificationModel) =>
            _notifications =
                _notifications.Select(
                    notification =>
                        notification == notificationModel
                            ? (notification with { IsDismissed = true }) : notification)
                    .ToHashSet();

        async ValueTask IAsyncDisposable.DisposeAsync()
        {
            if (Logger.IsEnabled(LogLevel.Information))
            {
                Logger.LogInformation("Disposing of NotificationComponent.razor.cs");
            }

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
