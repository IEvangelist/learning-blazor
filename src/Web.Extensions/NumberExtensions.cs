// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Extensions;

public static class NumberExtensions
{
    private static readonly string[] s_cardinals =
        { "N", "NE", "E", "SE", "S", "SW", "W", "NW", "N" };

    private static readonly string[] s_verboseCardinals =
        { "North", "Northeast", "East", "Southeast", "South", "Southwest", "West", "Northwest", "North" };

    private static readonly DateTime s_epochDateTime =
        new(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

    public static DateTime FromUnixTimeStamp(this double unixTimeStamp) =>
        s_epochDateTime.AddSeconds(unixTimeStamp).ToLocalTime();

    public static string ToPositionalCardinal(this int degrees) =>
        s_cardinals[(int)Math.Round((double)degrees % 360 / 45)];

    public static string ToVerbosePositionalCardinal(this int degrees) =>
        s_verboseCardinals[(int)Math.Round((double)degrees % 360 / 45)];
}
