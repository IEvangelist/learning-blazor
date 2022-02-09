// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor;

public sealed partial class SharedHubConnection
{
    public async Task StartAsync(
        ComponentBase component, CancellationToken token = default)
    {
        await _lock.WaitAsync(token);

        try
        {
            if (State is HubConnectionState.Disconnected)
            {
                await _hubConnection.StartAsync(token);
            }
            else
            {
                _logger.LogUnableToStartHubConnection(
                    component.GetType(),
                    _activeComponents.Count,
                    State);
            }
        }
        finally
        {
            _lock.Release();
        }
    }
}
