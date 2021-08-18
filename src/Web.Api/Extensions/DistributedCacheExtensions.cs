// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using Learning.Blazor.Extensions;
using Microsoft.Extensions.Caching.Distributed;
using static System.Text.Encoding;

namespace Learning.Blazor.Api.Extensions;

internal static class DistributedCacheExtensions
{
    /// <summary>
    /// The default <see cref="DistributedCacheEntryOptions.AbsoluteExpirationRelativeToNow"/> is 5 mins.
    /// </summary>
    internal static async Task<TItem?> GetOrCreateAsync<TItem>(
        this IDistributedCache distributedCache,
        string key,
        Func<DistributedCacheEntryOptions, Task<TItem>> factory,
        ILogger logger,
        CancellationToken token = default)
        where TItem : class
    {
        TItem? result;

        var bytes = await TryAsync(() => distributedCache.GetAsync(key, token), logger);
        if (bytes is { Length: > 0 })
        {
            var json = UTF8.GetString(bytes);
            result = json.FromJson<TItem>()!;
        }
        else
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            }.SetSlidingExpiration(TimeSpan.FromMinutes(1));

            result = await TryAsync<TItem>(() => factory(options), logger);

            var json = result.ToJson()!;
            bytes = UTF8.GetBytes(json);

            try
            {
                await distributedCache.SetAsync(key, bytes, options, token);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }
        }

        return result!;

        static async Task<T?> TryAsync<T>(Func<Task<T>> funcTask, ILogger logger)
        {
            try
            {
                return await (funcTask?.Invoke() ?? Task.FromResult<T>(default!));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
            }

            return default;
        }
    }
}
