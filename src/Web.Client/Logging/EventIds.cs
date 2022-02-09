// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Logging;

static class EventIds
{
    internal static readonly EventId TodoTaskCreated =
        new(id: 7_000, name: nameof(TodoTaskCreated));

    internal static readonly EventId TodoTaskUpdated =
        new(id: 7_001, name: nameof(TodoTaskUpdated));

    internal static readonly EventId TodoTaskDeleted =
        new(id: 7_002, name: nameof(TodoTaskDeleted));

    internal static readonly EventId UnableToStartSharedHubConnection =
        new(id: 8_000, name: nameof(UnableToStartSharedHubConnection));

    internal static readonly EventId UnableToStopSharedHubConnection =
        new(id: 8_001, name: nameof(UnableToStopSharedHubConnection));

    internal static readonly EventId UnableToGetAccessToken =
        new(id: 8_002, name: nameof(UnableToGetAccessToken));

    internal static readonly EventId HubConnectionClosed =
        new(id: 8_003, name: nameof(HubConnectionClosed));

    internal static readonly EventId HubConnectionReconnected =
        new(id: 8_004, name: nameof(HubConnectionReconnected));

    internal static readonly EventId HubConnectionReconnecting =
        new(id: 8_005, name: nameof(HubConnectionReconnecting));
}
