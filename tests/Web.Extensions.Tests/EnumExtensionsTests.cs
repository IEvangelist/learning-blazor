// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Extensions.Tests;

public class EnumExtensionsTests
{
    [
        Theory,
        InlineData("friday", DayOfWeek.Friday),
        InlineData("MONDAY", DayOfWeek.Monday),
        InlineData("pickles!", default(DayOfWeek)),
    ]
    public void ToEnumCorrectlyParsesTest(string enumString, DayOfWeek expected)
    {
        var actual = enumString.ToEnum<DayOfWeek>();
        Assert.Equal(expected, actual);
    }
}
