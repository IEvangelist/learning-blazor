// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using Learning.Blazor.Extensions;
using Learning.Blazor.Models;

namespace Learning.Blazor.Serialization
{
    [JsonSerializable(typeof(WeatherLanguage[]))]
    internal partial class WeatherLanguagesJsonSerializerContext
        : JsonSerializerContext
    {
        internal static JsonTypeInfo<WeatherLanguage[]> DefaultTypeInfo =>
            new WeatherLanguagesJsonSerializerContext(
                DefaultJsonSerialization.Options).WeatherLanguageArray;
    }
}
