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
        static readonly JsonSerializerOptions s_options = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public static string? ToJson<T>(
            this T value, JsonSerializerOptions? options = null) where T : class =>
            value is null ? null : Serialize(value, options ?? s_options);

        public static T? FromJson<T>(
            this string? json, JsonSerializerOptions? options = null) where T : class =>
            json is null or { Length: 0 } ? default : Deserialize<T>(json, options ?? s_options);

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
