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
}
