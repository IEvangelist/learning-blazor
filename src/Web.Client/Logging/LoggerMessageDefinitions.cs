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

    internal static readonly Action<ILogger, Guid, Type, int, HubConnectionState, Exception?> UnableToStartHubConnection =
        LoggerMessage.Define<Guid, Type, int, HubConnectionState>(
            LogLevel.Debug,
            EventIds.UnableToStartSharedHubConnection,
            "{Id}: {Type} requested start, unable to start. " +
            "Active component count: {Count}, and connection state: {State}");

    internal static readonly Action<ILogger, Guid, Type, int, HubConnectionState, Exception?> UnableToStopHubConnection =
        LoggerMessage.Define<Guid, Type, int, HubConnectionState>(
            LogLevel.Debug,
            EventIds.UnableToStopSharedHubConnection,
            "{Id}: {Type} requested stop, unable to stop. " +
            "Active component count: {Count}, and connection state: {State}");

    internal static readonly Action<ILogger, AccessTokenResultStatus, string, Exception?> UnableToGetAccessToken =
        LoggerMessage.Define<AccessTokenResultStatus, string>(
            LogLevel.Warning,
            EventIds.UnableToGetAccessToken,
            "Unable to get the access token. '{Status}' - Return URL: {Url}");
}
