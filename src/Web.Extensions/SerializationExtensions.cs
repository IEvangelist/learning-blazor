// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Extensions;

public static class SerializationExtensions
{
    public static string? ToJson<T>(
        this T value, JsonSerializerOptions? options = null)
        where T : class =>
        value is not null
            ? Serialize(value, options ?? DefaultJsonSerialization.Options)
            : default;

    public static T? FromJson<T>(
        this string? json, JsonSerializerOptions? options = null)
        where T : class =>
        json is { Length: > 0 }
            ? Deserialize<T>(json, options ?? DefaultJsonSerialization.Options)
            : default;
}
