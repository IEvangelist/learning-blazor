// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using Xunit;

namespace Learning.Blazor.Extensions.Tests
{
    public class NumberExtensionsTests
    {
        [Fact(Skip = "The server is probably messing this updae with date time offsets, need to investigate.")]
        public void FromUnixTimeStampCorrectlyCreatesDateTime()
        {
            var timeStamp = 458060838.0;
            var actual = timeStamp.FromUnixTimeStamp();

            Assert.Equal(new DateTime(1984, 7, 7, 10, 7, 18, DateTimeKind.Local), actual);
        }

        [
            Theory,
            InlineData(3, "N"),
            InlineData(47, "NE"),
            InlineData(89, "E"),
            InlineData(129, "SE"),
            InlineData(158, "S"),
            InlineData(210, "SW"),
            InlineData(270, "W"),
            InlineData(320, "NW"),
        ]
        public void ToCardinalCorrectlyPlotsDirectionAcronymTest(int cardinal, string expected)
        {
            var actual = cardinal.ToCardinal();
            Assert.Equal(expected, actual);
        }

        [
            Theory,
            InlineData(19, "North"),
            InlineData(56, "Northeast"),
            InlineData(94, "East"),
            InlineData(117, "Southeast"),
            InlineData(163, "South"),
            InlineData(208, "Southwest"),
            InlineData(285, "West"),
            InlineData(333, "Northwest"),
        ]
        public void ToVerboseCardinalCorrectlyPlotsDirectionAcronymTest(int cardinal, string expected)
        {
            var actual = cardinal.ToVerboseCardinal();
            Assert.Equal(expected, actual);
        }
    }
}
