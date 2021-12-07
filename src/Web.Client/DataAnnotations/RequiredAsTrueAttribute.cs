// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System.ComponentModel.DataAnnotations;

namespace Learning.Blazor.DataAnnotations;

[
    AttributeUsage(
        AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter,
        AllowMultiple = false)
]
public sealed class RequiredAsAttribute<T>
    : DataTypeAttribute where T : struct
{
    /// <summary>
    /// Gets or sets the value to compare to for validity checking.
    /// </summary>
    public T RequiredValue { get; set; }

    public RequiredAsAttribute(T requiredValue) : base(DataType.Custom) =>
        RequiredValue = requiredValue;

    public override bool IsValid(object? value)
    {
        if (value is null)
        {
            return false;
        }

        return RequiredValue.Equals(value);
    }
}
