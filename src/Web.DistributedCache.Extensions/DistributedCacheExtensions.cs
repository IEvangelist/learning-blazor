// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.DistributedCache.Extensions;

public static class DistributedCacheExtensions
{
    /// <summary>
    /// The default <see cref="DistributedCacheEntryOptions.AbsoluteExpirationRelativeToNow"/> is 5 mins.
    /// This is an abstraction over the otherwise obscure distributed cache APIs.
    /// It's a multi-step processing function:
    /// <list type="bullet">
    /// <item>Attempt to get the value from the distributed cache:</item>
    /// <item>If there is a value for the given <paramref name="key"/>,
    /// deserialize it into an <typeparamref name="TItem"/> instance.</item>
    /// <item>When there is no value for the given <paramref name="key"/>,
    /// attempt to create an instance of <typeparamref name="TItem"/>
    /// with the given <paramref name="factory"/> method.</item>
    /// <item>Update the value in the <paramref name="distributedCache"/>
    /// if we used the <paramref name="factory"/> to create an instance</item>
    /// </list>
    /// <remarks>
    /// There is no locking, but there is the potential for multiple read calls.
    /// Not too concerned about the potential for those cases to cause performance issues.
    /// </remarks>
    /// </summary>
    public static async Task<TItem?> GetOrCreateAsync<TItem>(
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

            var json = result?.ToJson()!;
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
