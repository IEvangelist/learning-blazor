// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Logging;

static class LoggerExtensions
{
    internal static void LogTodoCreated(
        this ILogger logger,
        TodoItem todo) =>
        LoggerMessageDefinitions.TodoTaskedCreated(logger, todo.Id, todo, null);

    internal static void LogTodoUpdated(
        this ILogger logger,
        TodoItem todo) =>
        LoggerMessageDefinitions.TodoTaskedUpdated(logger, todo.Id, todo, null);

    internal static void LogTodoDeleted(
        this ILogger logger,
        TodoItem todo) =>
        LoggerMessageDefinitions.TodoTaskedDeleted(logger, todo.Id, todo, null);

    internal static void LogUnableToStartHubConnection(
        this ILogger logger,
        HubConnectionState state) =>
        LoggerMessageDefinitions.UnableToStartHubConnection(
            logger, state, null);

    internal static void LogUnableToGetAccessToken(
        this ILogger logger,
        AccessTokenResultStatus status,
        string url) =>
        LoggerMessageDefinitions.UnableToGetAccessToken(
            logger, status, url, null);

    internal static void LogHubConnectionClosed(
        this ILogger logger,
        Exception? exception) =>
        LoggerMessageDefinitions.HubConnectionClosed(
            logger, exception);

    internal static void LogHubConnectionReconnecting(
        this ILogger logger,
        Exception? exception) =>
        LoggerMessageDefinitions.HubConnectionReconnecting(
            logger, exception);

    internal static void LogHubConnectionReconnected(
        this ILogger logger,
        string? message) =>
        LoggerMessageDefinitions.HubConnectionReconnected(
            logger, message, null);
}
