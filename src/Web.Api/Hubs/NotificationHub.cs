// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Api.Hubs;

[Authorize, RequiredScope(["User.ApiAccess"])]
public partial class NotificationHub(
    ITwitterService twitterService,
    IStringLocalizer<Shared> localizer) : Hub
{
    private string _userName => Context.User?.Identity?.Name ?? "Unknown";
    private string[]? _userEmail => Context.User?.GetEmailAddresses();

    public override Task OnConnectedAsync() =>
        Clients.All.SendAsync(
            HubServerEventNames.UserLoggedIn,
            Notification<Actor>.FromAlert(
                new(UserName: _userName ?? "Unknown",
                    Emails: _userEmail)));

    public override Task OnDisconnectedAsync(Exception? ex) =>
        Clients.All.SendAsync(
            HubServerEventNames.UserLoggedOut,
            Notification<Actor>.FromAlert(
                new(UserName: _userName)));

    /* Additional notification hub functionality 
     * defined in TwitterWorkerService.cs:
     *      TweetReceived
     *      StatusUpdated
     */
}
