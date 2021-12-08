// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System.ComponentModel.DataAnnotations;

namespace Learning.Blazor.DataAnnotations;

[
    AttributeUsage(
        AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter,
        AllowMultiple = false)
]
public sealed class RequiredAcceptanceAttribute : DataTypeAttribute
{
    public RequiredAcceptanceAttribute()
        : base(DataType.Custom)
    {
    }

    public override bool IsValid(object? value)
    {
        if (value is null)
        {
            return false;
        }

        return bool.TryParse(value.ToString(), out var result)
            && result;
    }
}
