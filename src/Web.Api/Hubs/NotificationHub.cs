// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Api.Hubs;

[Authorize, RequiredScope(new[] { "User.ApiAccess" })]
public class NotificationHub : Hub
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
        (_twitterService, _localizer, _logger) = (twitterService, localizer, logger);

    public override Task OnConnectedAsync() =>
        Clients.All.SendAsync(
            HubServerEventNames.UserLoggedIn,
            Notification<Actor>.FromAlert(
                new(_userName ?? "Unknown", _userEmail)));

    public override Task OnDisconnectedAsync(Exception? ex) =>
        Clients.All.SendAsync(
            HubServerEventNames.UserLoggedOut,
            Notification<Actor>.FromAlert(
                new(_userName ?? "Unknown")));

    public Task ToggleUserTyping(bool isTyping) =>
        Clients.Others.SendAsync(
            HubServerEventNames.UserTyping,
            Notification<ActorAction>.FromAlert(
                new(_userName ?? "Unknown", isTyping)));

    public Task PostOrUpdateMessage(string room, string message, Guid? id = default!) =>
        Clients.Groups(room).SendAsync(
            HubServerEventNames.MessageReceived,
            Notification<ActorMessage>.FromChat(
                new(id ?? Guid.NewGuid(), message, _userName ?? "Unknown", IsEdit: id.HasValue)));

    public async Task JoinTweets()
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, HubGroupNames.Tweets);

        if (_twitterService.CurrentStatus is StreamingStatus status)
        {
            _logger.LogInformation(
                "Replaying streaming status = {IsStreaming}.",
                status.IsStreaming);

            await Clients.Caller.SendAsync(
                HubServerEventNames.StatusUpdated,
                Notification<StreamingStatus>.FromStatus(
                    _twitterService.CurrentStatus));
        }
        else
        {
            _logger.LogInformation(
                "No current streaming status available, unable to replay.");
        }

        if (_twitterService.LastFiftyTweets is { Count: > 0 })
        {
            _logger.LogInformation(
                "Replaying last {Count} tweet.",
                _twitterService.LastFiftyTweets.Count);

            await Clients.Caller.SendAsync(
                HubServerEventNames.InitialTweetsLoaded,
                Notification<List<TweetContents>>.FromTweets(
                    _twitterService.LastFiftyTweets.ToList()));
        }
        else
        {
            _logger.LogInformation(
                "No previous tweets available, unable to replay.");
        }
    }

    public Task LeaveTweets() =>
        Groups.RemoveFromGroupAsync(Context.ConnectionId, HubGroupNames.Tweets);

    public Task StartTweetStream() =>
        _twitterService.StartTweetStreamAsync();

    public async Task JoinChat(string room)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, room);
        await Clients.Caller.SendAsync(
            HubServerEventNames.MessageReceived,
            Notification<ActorMessage>.FromChat(
                new(Id: Guid.NewGuid(), Text: _localizer["WelcomeToChatRoom", room],
                    UserName: "👋", IsGreeting: true)));
    }

    public async Task LeaveChat(string room)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, room);
        await Clients.Groups(room).SendAsync(
            HubServerEventNames.MessageReceived,
            Notification<ActorMessage>.FromChat(
                new(Id: Guid.NewGuid(), Text: _localizer["HasLeftTheChatRoom", _userName ?? "?"],
                    UserName: "🤖")));
    }

    /* Additional notification hub functionality 
     * defined in TwitterWorkerService.cs:
     *      TweetReceived
     *      StatusUpdated
     */
}
