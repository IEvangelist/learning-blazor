// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Extensions;

public static class DefaultJsonSerialization
{
    // The code generator bits do NOT seem to care much for sharing this.
    // We cannot use a `{ get; } = new()`, instead we have to new it up every time.
    public static JsonSerializerOptions Options => new(JsonSerializerDefaults.Web)
    {
        Converters =
        {
            new JsonStringEnumConverter()
        }
    };
}
