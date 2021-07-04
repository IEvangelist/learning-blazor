// Copyright (c) 2021 David Pine. All rights reserved.
//  Licensed under the MIT License.

using System;

namespace Learning.Blazor.Extensions
{
    public static class DoubleExtensions
    {
        private static readonly DateTime s_epochDateTime =
            new(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        public static DateTime FromUnixTimeStamp(this double unixTimeStamp) =>
            s_epochDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
    }
}
