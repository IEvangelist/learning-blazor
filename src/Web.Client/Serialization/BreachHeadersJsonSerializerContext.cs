// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization.Metadata;

namespace Learning.Blazor.Serialization
{
    [JsonSerializable(typeof(BreachHeader[]))]
    internal partial class BreachHeadersJsonSerializerContext
        : JsonSerializerContext
    {
        internal static JsonTypeInfo<BreachHeader[]> DefaultTypeInfo =>
            new BreachHeadersJsonSerializerContext(
                DefaultJsonSerialization.Options).BreachHeaderArray;
    }
}
