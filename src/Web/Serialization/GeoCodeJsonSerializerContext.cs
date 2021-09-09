// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using Learning.Blazor.Extensions;
using Learning.Blazor.Models;

namespace Learning.Blazor.Serialization
{
    [JsonSerializable(typeof(GeoCode))]
    internal partial class GeoCodeJsonSerializerContext
        : JsonSerializerContext
    {
        internal static JsonTypeInfo<GeoCode> DefaultTypeInfo =>
            new GeoCodeJsonSerializerContext(
                DefaultJsonSerialization.Options).GeoCode;
    }
}
