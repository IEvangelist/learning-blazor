// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor;

public partial class SharedHubConnection
{
    public async Task StartAsync(
        ComponentBase component, CancellationToken token = default)
    {
        await _lock.WaitAsync(token);

        try
        {
            _ = _activeComponents.Add(component);
            if (_activeComponents.Count > 0 &&
                State is HubConnectionState.Disconnected)
            {
                await _asyncRetryPolicy.ExecuteAsync(
                    async cancellationToken =>
                        await _hubConnection.StartAsync(
                            cancellationToken),
                    token);
            }
            else
            {
                _logger.LogUnableToStartHubConnection(
                    _id,
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

    public async Task StopAsync(
        ComponentBase component, CancellationToken token = default)
    {
        await _lock.WaitAsync(token);

        try
        {
            _ = _activeComponents.Remove(component);
            if (_activeComponents.Count is 0 &&
                State is not HubConnectionState.Disconnected)
            {
                await _hubConnection.StopAsync(token);
            }
            else
            {
                _logger.LogUnableToStopHubConnection(
                    _id,
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
