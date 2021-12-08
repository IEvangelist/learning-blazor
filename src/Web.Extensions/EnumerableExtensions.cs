// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using SystemRandom = System.Random;

namespace Learning.Blazor.Extensions;

public static class EnumerableExtensions
{
    static readonly SystemRandom s_random = SystemRandom.Shared;

    public static T RandomElement<T>(
        this IEnumerable<T> source) =>
        source.ElementAt(s_random.Next(source.Count()));

    public static IEnumerable<T> Random<T>(
        this IEnumerable<T> source, int count = 1) =>
        source?.OrderBy(_ => s_random.Next())?.Take(count) ?? Array.Empty<T>();
}
