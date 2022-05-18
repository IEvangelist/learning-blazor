// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using SystemRandom = System.Random;

namespace Learning.Blazor.Extensions;

public static class EnumerableExtensions
{
    static readonly SystemRandom s_random = SystemRandom.Shared;

    public static async Task<TResult> UseRandomAsync<T, TResult>(
        this IList<T> source,
        Func<bool> hasMore,
        Func<int, bool> tryVisit,
        Func<T, Task<(bool UsedSuccessfully, TResult Result)>> tryUseAsync,
        TResult defaultValue)
    {
        while (hasMore())
        {
            var index = s_random.Next(source.Count);
            if (tryVisit(index) is false)
            {
                continue;
            }

            var (usedSuccessfully, result) = await tryUseAsync(source[index]);
            if (usedSuccessfully)
            {
                return result;
            }
        }

        return await Task.FromResult(defaultValue);
    }

    public static string? ToSpaceDelimitedString(
        this string[]? source) =>
        string.Join(" ", source ?? Array.Empty<string>());
}
