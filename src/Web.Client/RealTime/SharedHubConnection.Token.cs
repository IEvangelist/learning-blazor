// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor;

public partial class SharedHubConnection
{
    private async Task<string?> GetAccessTokenValueAsync()
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var tokenProvider =
                scope.ServiceProvider.GetRequiredService<IAccessTokenProvider>();
            var result =
                await tokenProvider.RequestAccessToken();

            if (result.TryGetToken(out var accessToken))
            {
                return accessToken.Value;
            }

            _logger.LogUnableToGetAccessToken(
                result.Status, result.RedirectUrl);

            return null;
        }
    }
}
