// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using SystemRandom = System.Random;

namespace Learning.Blazor.Extensions;

public static class EnumerableExtensions
{
    static readonly SystemRandom s_random = SystemRandom.Shared;

    public static IEnumerable<T> RandomOrder<T>(this IList<T> incoming)
    {
        var used = new HashSet<int>();
        var count = incoming.Count;

        while (used.Count != count)
        {
            var index = s_random.Next(incoming.Count);
            if (!used.Add(index))
            {
                continue;
            }

            yield return incoming[index];
        }

        yield break;
    }

    public static string? ToSpaceDelimitedString(
        this string[]? source) =>
        string.Join(" ", source ?? Array.Empty<string>());
}
