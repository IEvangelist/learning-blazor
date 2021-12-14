// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization.Metadata;

namespace Learning.Blazor.Serialization
{
    [JsonSerializable(typeof(JokeResponse))]
    internal partial class JokeResponseJsonSerializerContext
        : JsonSerializerContext
    {
        internal static JsonTypeInfo<JokeResponse> DefaultTypeInfo =>
            new JokeResponseJsonSerializerContext(
                DefaultJsonSerialization.Options).JokeResponse;
    }
}
