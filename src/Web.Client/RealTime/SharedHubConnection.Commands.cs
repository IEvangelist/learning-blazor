// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor;

public sealed partial class SharedHubConnection
{
    public async Task StartAsync(CancellationToken token = default)
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
                _logger.LogUnableToStartHubConnection(State);
            }
        }
        finally
        {
            _lock.Release();
        }
    }
}
