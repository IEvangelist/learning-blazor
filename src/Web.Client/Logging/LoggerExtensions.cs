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
}
