// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.PwnedApi.Services;

public class PwnedServices
{
    private readonly IDistributedCache _cache;
    private readonly IPwnedBreachesClient _pwnedBreachesClient;
    private readonly ILogger<PwnedServices> _logger;

    public PwnedServices(
        IDistributedCache cache,
        IPwnedBreachesClient pwnedBreachesClient,
        ILogger<PwnedServices> logger) =>
        (_cache, _pwnedBreachesClient, _logger) = (cache, pwnedBreachesClient, logger);

    public async Task<BreachHeader[]?> GetBreachHeadersAsync(string email)
    {
        var breaches = await _cache.GetOrCreateAsync(
            $"breach:{email}",
            async options =>
            {
                options.AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1);
                return await _pwnedBreachesClient.GetBreachHeadersForAccountAsync(email);
            },
            _logger);

        return breaches;
    }

    public async Task<BreachDetails?> GetBreachDetailsAsync(string name)
    {
        var breach = await _cache.GetOrCreateAsync(
            name,
            async options =>
            {
                options.AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(7);
                var breach = await _pwnedBreachesClient.GetBreachAsync(name);
                return breach!;
            },
            _logger);

        return breach;
    }
}
