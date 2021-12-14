// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Extensions;

/// <summary>
/// Inspired by: https://chrissainty.com/working-with-query-strings-in-blazor
/// </summary>
internal static class NavigationManagerExtensions
{
    internal static bool TryParseQueryEnum<T>(
        this NavigationManager navManager, string key, out T value) where T : struct
    {
        var uri = navManager.ToAbsoluteUri(navManager.Uri);
        if (QueryHelpers.ParseQuery(uri.Query).TryGetValue(key, out var valueFromQueryString))
        {
            if (typeof(T).IsEnum && Enum.TryParse<T>(valueFromQueryString, out var valueAsEnum))
            {
                value = valueAsEnum;
                return true;
            }
        }

        value = default!;
        return false;
    }

    internal static bool TryParseQueryString<T>(
        this NavigationManager navManager, string key, out T value)
    {
        var uri = navManager.ToAbsoluteUri(navManager.Uri);
        if (QueryHelpers.ParseQuery(uri.Query).TryGetValue(key, out var valueFromQueryString))
        {
            var type = typeof(T);
            if (type == typeof(int) && int.TryParse(valueFromQueryString, out var valueAsInt))
            {
                value = (T)(object)valueAsInt;
                return true;
            }

            if (type == typeof(string))
            {
                value = (T)(object)valueFromQueryString.ToString();
                return true;
            }

            if (type == typeof(decimal) && decimal.TryParse(valueFromQueryString, out var valueAsDecimal))
            {
                value = (T)(object)valueAsDecimal;
                return true;
            }
        }

        value = default!;
        return false;
    }
}
