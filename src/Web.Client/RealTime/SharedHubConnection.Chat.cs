// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor;

public sealed partial class SharedHubConnection
{
    /// <inheritdoc cref="HubClientMethodNames.JoinChat" />
    public Task JoinChatAsync(string room) =>
        _hubConnection.InvokeAsync(
            methodName: HubClientMethodNames.JoinChat, room);

    /// <inheritdoc cref="HubClientMethodNames.LeaveChat" />
    public Task LeaveChatAsync(string room) =>
        _hubConnection.InvokeAsync(
            methodName: HubClientMethodNames.LeaveChat, room);

    /// <inheritdoc cref="HubClientMethodNames.PostOrUpdateMessage" />
    public Task PostOrUpdateMessageAsync(
        string room, string message, Guid? id = default) =>
        _hubConnection.InvokeAsync(
            methodName: HubClientMethodNames.PostOrUpdateMessage,
            room, message, id);

    /// <inheritdoc cref="HubClientMethodNames.ToggleUserTyping" />
    public Task ToggleUserTypingAsync(bool isTyping) =>
        _hubConnection.InvokeAsync(
            methodName: HubClientMethodNames.ToggleUserTyping, isTyping);

    /// <inheritdoc cref="HubServerEventNames.UserLoggedIn" />
    public IDisposable SubscribeToUserLoggedIn(
        Func<Notification<Actor>, Task> onUserLoggedIn) =>
        _hubConnection.On(
            methodName: HubServerEventNames.UserLoggedIn,
            handler: onUserLoggedIn);

    /// <inheritdoc cref="HubServerEventNames.UserLoggedOut" />
    public IDisposable SubscribeToUserLoggedOut(
        Func<Notification<Actor>, Task> onUserLoggedOut) =>
        _hubConnection.On(
            methodName: HubServerEventNames.UserLoggedOut,
            handler: onUserLoggedOut);

    /// <inheritdoc cref="HubServerEventNames.UserTyping" />
    public IDisposable SubscribeToUserTyping(
        Func<Notification<ActorAction>, Task> onUserTyping) =>
        _hubConnection.On(
            methodName: HubServerEventNames.UserTyping,
            handler: onUserTyping);

    /// <inheritdoc cref="HubServerEventNames.MessageReceived" />
    public IDisposable SubscribeToMessageReceived(
        Func<Notification<ActorMessage>, Task> onMessageReceived) =>
        _hubConnection.On(
            methodName: HubServerEventNames.MessageReceived,
            handler: onMessageReceived);
}
