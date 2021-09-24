// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using Xunit;

namespace Learning.Blazor.Extensions.Tests
{
    public class OobjectExtensionsTests
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

        [Fact(Skip = "System.IO.FileNotFoundException : Could not load file or assembly 'Microsoft.Bcl.AsyncInterfaces, Version=1.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51'. The system cannot find the file specified.")]
        public async Task TryDisposeAsyncCorrectlyCallsDisposeAsync()
        {
            VerifiableAsyncDisposable verifiableAsyncDisposable = new();
            await verifiableAsyncDisposable.TryDisposeAsync();

            Assert.True(verifiableAsyncDisposable.IsDiposed);
        }

        [Fact(Skip = "System.IO.FileNotFoundException : Could not load file or assembly 'Microsoft.Bcl.AsyncInterfaces, Version=1.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51'. The system cannot find the file specified.")]
        public async Task TryDisposeAsyncCorrectlyCallsDispose()
        {
            VerifiableDisposable verifiableDisposable = new();
            await verifiableDisposable.TryDisposeAsync();

            Assert.True(verifiableDisposable.IsDiposed);
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

    internal class VerifiableAsyncDisposable : IAsyncDisposable
    {
        public bool IsDiposed { get; private set; } = false;

        public ValueTask DisposeAsync()
        {
            IsDiposed = true;

            return ValueTask.CompletedTask;
        }
    }

    internal class VerifiableDisposable : IDisposable
    {
        public bool IsDiposed { get; private set; } = false;

        public void Dispose() => IsDiposed = true;
    }
}
