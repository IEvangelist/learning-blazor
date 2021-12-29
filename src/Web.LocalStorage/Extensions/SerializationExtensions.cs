// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.LocalStorage;

internal static class SerializationExtensions
{
    internal static string? TryToJson<TItem>(
        this TItem item, ILogger logger)
        where TItem : class
    {
        try
        {
            return item.ToJson();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
        }

        return default;
    }

    internal static TItem? TryFromJson<TItem>(
        this string json, ILogger logger)
        where TItem : class
    {
        try
        {
            return json.FromJson<TItem>();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
        }

        return default;
    }
}
