// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Learning.Blazor.DataAnnotations;

[
    AttributeUsage(
        AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter,
        AllowMultiple = false)
]
public sealed class RegexEmailAddressAttribute : DataTypeAttribute
{
    private readonly Regex _expression =
        new(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
            RegexOptions.CultureInvariant | RegexOptions.Singleline);

    /// <summary>
    /// Gets or sets a value indicating if an email is required.
    /// </summary>
    /// <remarks>Defaults to <c>true</c>.</remarks>
    public bool IsRequired { get; set; } = true;

    public RegexEmailAddressAttribute()
        : base(DataType.EmailAddress)
    {
    }

    public override bool IsValid(object? value)
    {
        if (value is null)
        {
            return !IsRequired;
        }

        return value is string valueAsString
            && _expression.IsMatch(valueAsString);
    }
}
