// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization.Metadata;

namespace Learning.Blazor.Serialization
{
    [JsonSerializable(typeof(WeatherDetails))]
    internal partial class WeatherDetailsJsonSerializerContext
        : JsonSerializerContext
    {
        internal static JsonTypeInfo<WeatherDetails> DefaultTypeInfo =>
            new WeatherDetailsJsonSerializerContext(
                DefaultJsonSerialization.Options).WeatherDetails;
    }
}
