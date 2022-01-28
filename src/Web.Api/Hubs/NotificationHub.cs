// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Api.Hubs;

[Authorize, RequiredScope(new[] { "User.ApiAccess" })]
public partial class NotificationHub : Hub
{
    private readonly ITwitterService _twitterService;
    private readonly IStringLocalizer<Shared> _localizer;
    private readonly ILogger<NotificationHub> _logger;

    private string? _userName => Context?.User?.Identity?.Name;
    private string[]? _userEmail => Context?.User?.GetEmailAddresses();

    public NotificationHub(
        ITwitterService twitterService,
        IStringLocalizer<Shared> localizer,
        ILogger<NotificationHub> logger) =>
        (_twitterService, _localizer, _logger) =
            (twitterService, localizer, logger);

    public override Task OnConnectedAsync() =>
        Clients.All.SendAsync(
            HubServerEventNames.UserLoggedIn,
            Notification<Actor>.FromAlert(
                new(
                    UserName: _userName ?? "Unknown",
                    Emails: _userEmail)));

    public override Task OnDisconnectedAsync(Exception? ex) =>
        Clients.All.SendAsync(
            HubServerEventNames.UserLoggedOut,
            Notification<Actor>.FromAlert(
                new(
                    UserName: _userName ?? "Unknown")));

    /* Additional notification hub functionality 
     * defined in TwitterWorkerService.cs:
     *      TweetReceived
     *      StatusUpdated
     */
}
