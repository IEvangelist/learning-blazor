﻿// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Components;

public sealed partial class NotificationComponent : IDisposable
{
    private readonly Stack<IDisposable> _subscriptions = new();

    private HashSet<NotificationComponentModel> _notifications =
        new(NotificationComponentModelComparer.Instance);

    private bool _show = false;
    private DateTime? _latestNotificationDateTime = null!;

    private string _showClass => _show ? "is-active" : "";

    [Inject]
    public IHttpClientFactory HttpFactory { get; set; } = null!;

    [Inject]
    public SharedHubConnection HubConnection { get; set; } = null!;

    [Inject]
    public NavigationManager Navigation { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        _subscriptions.Push(
            HubConnection.SubscribeToUserLoggedIn(
                OnUserLoggedInAsync));
        _subscriptions.Push(
            HubConnection.SubscribeToUserLoggedOut(
                OnUserLoggedOutAsync));

        await HubConnection.StartAsync();
    }

    protected override void OnParametersSet()
    {
        AppState.WeatherAlertReceived ??= OnWeatherAlertReceived;
        AppState.ContactPageSubmitted ??= OnContactPageSubmitted;
    }

    private Task OnUserLoggedInAsync(Notification<Actor> notification) =>
        InvokeAsync(async () =>
        {
            var emails = User?.GetEmailAddresses();

            Actor actor = notification;
            var intersectingEmails = actor.Emails?.Intersect(emails ?? [])?.ToList();
            if (intersectingEmails is { Count: > 0 })
            {
                var httpClient = HttpFactory.CreateClient(HttpClientNames.PwnedServerApi);
                foreach (var email in intersectingEmails)
                {
                    var breaches = (await httpClient.GetFromJsonAsync<BreachHeader[]>(
                        $"api/pwned/breaches/{email}",
                        BreachHeadersJsonSerializerContext.DefaultTypeInfo))
                        ?? [];
                    if (breaches is { Length: > 0 })
                    {
                        var uri = Navigation.ToAbsoluteUri($"/pwned/breaches?email={email}");

                        _ = _notifications.Add(new(
                            Localizer["EmailFoundInBreachFormat", email, breaches.Length],
                            NotificationType.Alert)
                        {
                            ContextualUri = uri
                        });
                    }
                }
            }
            else
            {
                var text = Localizer["UserLoggedInFormat", actor.UserName];
                _ = _notifications.Add(new(
                    text,
                    notification.Type));
            }

            _latestNotificationDateTime = DateTime.Now;

            StateHasChanged();
        });

    private Task OnUserLoggedOutAsync(Notification<Actor> notification) =>
        InvokeAsync(() =>
        {
            Actor actor = notification;
            var text = Localizer["UserLoggedOutFormat", actor.UserName];

            _ = _notifications.Add(new(
                text,
                notification.Type));

            _latestNotificationDateTime = DateTime.Now;

            StateHasChanged();
        });

    private void OnWeatherAlertReceived(IList<Alert> alerts)
    {
        foreach (var alert in alerts)
        {
            var text =
                $"{alert.Event}\n{alert.Description}\n— {alert.SenderName}";

            _ = _notifications.Add(
                new(
                    text, NotificationType.Warning));
        }

        StateHasChanged();
    }

    private void OnContactPageSubmitted(ContactComponentModel model)
    {
        var text =
            $"Thank you for your message, {model!.FirstName}. We will get back to you as soon as possible.";

        _ = _notifications.Add(
            new(
                text, NotificationType.Alert));

        StateHasChanged();
    }

    private void Show() => _show = true;

    private void Dismiss() => _show = false;

    private void DismissNotification(NotificationComponentModel dismissalNotification)
    {
        _notifications =
            _notifications.Select(
                notification =>
                    notification == dismissalNotification
                        ? notification with { IsDismissed = true }
                        : notification)
            .ToHashSet();

        _show = _notifications.Any(n => !n.IsDismissed);
    }

    void IDisposable.Dispose()
    {
        if (Logger.IsEnabled(LogLevel.Information))
        {
            Logger.LogInformation("Disposing of NotificationComponent.razor.cs");
        }

        while (_subscriptions.TryPop(out var disposable))
        {
            disposable.Dispose();
        }
    }
}
