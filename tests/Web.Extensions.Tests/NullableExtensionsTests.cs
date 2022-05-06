// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Extensions.Tests
{
    public class NullableExtensionsTests
    {
        [Fact]
        public void DeconstructCorrectlyEvaluatesNullableTest()
        {
            var questionableDateTime = new DateTime?();
            var (hasValue, value) = questionableDateTime;

            Assert.Equal((false, default(DateTime)), (hasValue, value));
        }

        [Fact]
        public void DeconstructCorrectlyEvaluatesNullableValueTest()
        {
            var now = DateTime.Now;
            var questionableDateTime = new DateTime?(now);
            var (hasValue, value) = questionableDateTime;

            Assert.Equal((true, now), (hasValue, value));
        }
    }
}
