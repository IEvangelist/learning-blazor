﻿// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Api.Hubs;

public partial class NotificationHub
{
    private int _userOnline;

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

    public async Task<int> JoinChat(string room)
    {
        await Groups.AddToGroupAsync(
            Context.ConnectionId, room);

        await Clients.Caller.SendAsync(
            HubServerEventNames.MessageReceived,
            Notification<ActorMessage>.FromChat(
                new(
                    Id: Guid.NewGuid(),
                    Text: localizer["WelcomeToChatRoom", room],
                    UserName: "🤖",
                    IsGreeting: true)));

        return Interlocked.Increment(ref _userOnline);
    }

    public async Task<int> LeaveChat(string room)
    {
        await Groups.RemoveFromGroupAsync(
            Context.ConnectionId, room);

        await Clients.Groups(room).SendAsync(
            HubServerEventNames.MessageReceived,
            Notification<ActorMessage>.FromChat(
                new(
                    Id: Guid.NewGuid(),
                    Text: localizer["HasLeftTheChatRoom", _userName ?? "?"],
                    UserName: "🤖")));

        return Interlocked.Decrement(ref _userOnline);
    }
}
