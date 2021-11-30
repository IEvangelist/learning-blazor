// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using Learning.Blazor.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace Learning.Blazor.ComponentModels;

public class ContactComponentModel
{
    [RegexEmailAddress(IsRequired = true)]
    public string? EmailAddress { get; set; } = null!;

    [Required]
    public string? Message { get; set; } = null!;
}
