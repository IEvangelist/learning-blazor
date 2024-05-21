// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.PwnedApi.Services;

public sealed class DefaultPwnedServices(
    IPwnedBreachesClient pwnedBreachesClient,
    ILogger<DefaultPwnedServices> logger) : IPwnedServices
{
    public async Task<BreachHeader[]?> GetBreachHeadersAsync(string email)
    {
        try
        {
            var breaches =
                await pwnedBreachesClient.GetBreachHeadersForAccountAsync(email);

            return breaches;
        }
        catch (Exception ex)
        {
            logger.LogError("{Error} {Msg}", ex, ex.Message);
            return default;
        }
    }

    public async Task<BreachDetails?> GetBreachDetailsAsync(string name)
    {
        try
        {
            var breach =
                await pwnedBreachesClient.GetBreachAsync(name);

            return breach;
        }
        catch (Exception ex)
        {
            logger.LogError("{Error} {Msg}", ex, ex.Message);
            return default;
        }
    }
}
