// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Extensions;

public static class EnumExtensions
{
    public static TEnum ToEnum<TEnum>(this string? value) where TEnum : struct =>
        Enum.TryParse(value, true, out TEnum result) ? result : default;
}
