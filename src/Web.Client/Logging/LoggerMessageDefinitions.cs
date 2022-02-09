// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Logging;

static class LoggerMessageDefinitions
{
    internal static readonly Action<ILogger, string, TodoItem, Exception?> TodoTaskedCreated =
        LoggerMessage.Define<string, TodoItem>(
            LogLevel.Debug,
            EventIds.TodoTaskCreated,
            "[{Id}]: A new todo was created: {Todo}");

    internal static readonly Action<ILogger, string, TodoItem, Exception?> TodoTaskedUpdated =
        LoggerMessage.Define<string, TodoItem>(
            LogLevel.Debug,
            EventIds.TodoTaskUpdated,
            "[{Id}]: A new todo was updated: {Todo}");

    internal static readonly Action<ILogger, string, TodoItem, Exception?> TodoTaskedDeleted =
        LoggerMessage.Define<string, TodoItem>(
            LogLevel.Debug,
            EventIds.TodoTaskDeleted,
            "[{Id}]: A new todo was deleted: {Todo}");

    internal static readonly Action<ILogger, HubConnectionState, Exception?> UnableToStartHubConnection =
        LoggerMessage.Define<HubConnectionState>(
            LogLevel.Debug,
            EventIds.UnableToStartSharedHubConnection,
            "StartAsync called, unable to start. " +
            "Current connection state: {State}");

    internal static readonly Action<ILogger, AccessTokenResultStatus, string, Exception?> UnableToGetAccessToken =
        LoggerMessage.Define<AccessTokenResultStatus, string>(
            LogLevel.Warning,
            EventIds.UnableToGetAccessToken,
            "Unable to get the access token. '{Status}' - Return URL: {Url}");

    internal static readonly Action<ILogger, Exception?> HubConnectionClosed =
        LoggerMessage.Define(
            LogLevel.Information,
            EventIds.HubConnectionClosed,
            "Hub connection closed");

    internal static readonly Action<ILogger, Exception?> HubConnectionReconnecting =
        LoggerMessage.Define(
            LogLevel.Information,
            EventIds.HubConnectionReconnecting,
            "Hub connection reconnecting");

    internal static readonly Action<ILogger, string?, Exception?> HubConnectionReconnected =
        LoggerMessage.Define<string?>(
            LogLevel.Information,
            EventIds.HubConnectionReconnected,
            "Hub connection reconnected: {Message}");
}
