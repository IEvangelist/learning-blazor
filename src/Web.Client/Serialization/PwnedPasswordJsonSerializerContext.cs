// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization.Metadata;

namespace Learning.Blazor.Serialization;

[JsonSerializable(typeof(PwnedPassword))]
internal partial class PwnedPasswordJsonSerializerContext
    : JsonSerializerContext
{
    internal static JsonTypeInfo<PwnedPassword> DefaultTypeInfo =>
        new PwnedPasswordJsonSerializerContext(
            DefaultJsonSerialization.Options).PwnedPassword;
}
