// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Api.Hubs;

public partial class NotificationHub
{
    public Task ToggleUserTyping(bool isTyping) =>
        Clients.Others.SendAsync(
            HubServerEventNames.UserTyping,
            Notification<ActorAction>.FromAlert(
                new(
                    UserName: _userName ?? "Unknown",
                    IsTyping: isTyping)));

    public Task PostOrUpdateMessage(
        string room, string message, Guid? id = default!) =>
        Clients.Groups(room).SendAsync(
            HubServerEventNames.MessageReceived,
            Notification<ActorMessage>.FromChat(
                new(
                   Id: id ?? Guid.NewGuid(),
                   Text: message,
                   UserName: _userName ?? "Unknown",
                   IsEdit: id.HasValue)));

    public async Task JoinChat(string room)
    {
        await Groups.AddToGroupAsync(
            Context.ConnectionId, room);

        await Clients.Caller.SendAsync(
            HubServerEventNames.MessageReceived,
            Notification<ActorMessage>.FromChat(
                new(
                    Id: Guid.NewGuid(),
                    Text: _localizer["WelcomeToChatRoom", room],
                    UserName: "👋",
                    IsGreeting: true)));
    }

    public async Task LeaveChat(string room)
    {
        await Groups.RemoveFromGroupAsync(
            Context.ConnectionId, room);

        await Clients.Groups(room).SendAsync(
            HubServerEventNames.MessageReceived,
            Notification<ActorMessage>.FromChat(
                new(
                    Id: Guid.NewGuid(),
                    Text: _localizer["HasLeftTheChatRoom", _userName ?? "?"],
                    UserName: "🤖")));
    }

}
