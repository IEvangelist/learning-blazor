// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using Xunit;

namespace Learning.Blazor.Extensions.Tests
{
    public class SerializationExtensionsTests
    {
        [Fact]
        public void ToJsonCorrectlySerializesTest()
        {
            var expected = new TestObject
            {
                Name = "System",
                Number = 7,
                Children = new[]
                {
                    new TestObject
                    {
                        Name = "Product",
                        Number = 8,
                        Children = new[]
                        {
                            new TestObject
                            {
                                Name = "Part",
                                Number = 9
                            },
                            new TestObject
                            {
                                Name = "Accessory",
                                Number = 10
                            }
                        }
                    }
                }
            };

            var json = expected.ToJson();
            var actual = json.FromJson<TestObject>();

            Assert.NotStrictEqual(expected, actual);
        }
    }

    internal record TestObject
    {
        public string? Name { get; init; }
        public int Number { get; init; }
        public Guid Id { get; init; } = Guid.NewGuid();
        public DateTime Date { get; init; } = DateTime.Now;
        public TestObject[] Children { get; init; } = Array.Empty<TestObject>();
    }
}
