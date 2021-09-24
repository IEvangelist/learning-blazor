// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Extensions;

public static class ObjectExtensions
{
    public static string? ToJson<T>(
        this T value, JsonSerializerOptions? options = null) =>
        value is not null
            ? Serialize(value, options ?? DefaultJsonSerialization.Options)
            : default;

    public static T? FromJson<T>(
        this string? json, JsonSerializerOptions? options = null) where T : class =>
        json is { Length: > 0 }
            ? Deserialize<T>(json, options ?? DefaultJsonSerialization.Options)
            : default;

    public static async ValueTask TryDisposeAsync(this object? obj)
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
