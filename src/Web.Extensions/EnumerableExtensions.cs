// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Learning.Blazor.Extensions;

public static class EnumerableExtensions
{
    static readonly Random s_random = new((int)DateTime.Now.Ticks);

    public static IEnumerable<T> Random<T>(this IEnumerable<T> source, int count = 1) =>
        source?.OrderBy(_ => s_random.Next())?.Take(count) ?? Array.Empty<T>();

    public static T RandomElement<T>(this IEnumerable<T> source) =>
        source.ElementAt(s_random.Next(source.Count()));
}
