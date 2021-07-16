// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Text.Json.JsonSerializer;

namespace Learning.Blazor.Extensions
{
    public static class ObjectExtensions
    {
        public static string? ToJson<T>(
            this T value, JsonSerializerOptions? options = null) =>
            value is null ? null : Serialize(value, options ?? DefaultJsonSerialization.Options);

        public static T? FromJson<T>(
            this string? json, JsonSerializerOptions? options = null) where T : class =>
            json is null or { Length: 0 } ? default : Deserialize<T>(json, options ?? DefaultJsonSerialization.Options);

        public static async ValueTask TryDisposeAsync(this object obj)
        {
            if (obj is IAsyncDisposable asyncDisposable)
            {
                await asyncDisposable.ConfigureAwait(false).DisposeAsync();
            }
            else if (obj is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }
}
