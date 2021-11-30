// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.PwnedApi.Services;

public class PwnedServices
{
    private readonly IPwnedBreachesClient _pwnedBreachesClient;
    private readonly ILogger<PwnedServices> _logger;

    public PwnedServices(
        IPwnedBreachesClient pwnedBreachesClient,
        ILogger<PwnedServices> logger) =>
        (_pwnedBreachesClient, _logger) = (pwnedBreachesClient, logger);

    public async Task<BreachHeader[]?> GetBreachHeadersAsync(string email)
    {
        try
        {
            var breaches =
                await _pwnedBreachesClient.GetBreachHeadersForAccountAsync(email);

            return breaches;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            return default;
        }
    }

    public async Task<BreachDetails?> GetBreachDetailsAsync(string name)
    {
        try
        {
            var breach =
                await _pwnedBreachesClient.GetBreachAsync(name);

            return breach;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            return default;
        }
    }
}
