// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Logging;

static class EventIds
{
    internal static readonly EventId TodoTaskCreated =
        new(
            id: 7_000,
            name: nameof(TodoTaskCreated));

    internal static readonly EventId TodoTaskUpdated =
        new(
            id: 7_001,
            name: nameof(TodoTaskUpdated));

    internal static readonly EventId TodoTaskDeleted =
        new(
            id: 7_002,
            name: nameof(TodoTaskDeleted));

    internal static readonly EventId UnableToStartSharedHubConnection =
        new(
            id: 8_000,
            name: nameof(UnableToStartSharedHubConnection));

    internal static readonly EventId UnableToStopSharedHubConnection =
        new(
            id: 8_001,
            name: nameof(UnableToStopSharedHubConnection));

    internal static readonly EventId UnableToGetAccessToken =
        new(
            id: 8_002,
            name: nameof(UnableToGetAccessToken));
}
