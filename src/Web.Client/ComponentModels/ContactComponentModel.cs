// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using Learning.Blazor.DataAnnotations;
using Learning.Blazor.Models;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Versioning;

namespace Learning.Blazor.ComponentModels;

public record ContactComponentModel()
{
    public string? FirstName { get; set; } = null!;

    public string? LastName { get; set; } = null!;

    [Required]
    public string? FullName
    {
        get
        {
            var (first, last) = (FirstName, LastName);

            return string.IsNullOrWhiteSpace(first)
                || string.IsNullOrWhiteSpace(last)
                    ? null
                    : $"{first} {last}"; 
        }
    }

    [RegexEmailAddress(IsRequired = true)]
    public string? EmailAddress { get; set; } = null!;

    [Required]
    public string? Subject { get; set; } = null!;

    [RequiredAs<bool>(true)]
    public bool AgreesToTerms { get; set; }

    [RequiresPreviewFeatures]
    public AreYouHumanMath<int> NotRobot { get; set; } // TODO: wire up in edit form.

    [Required]
    public string? Message { get; set; } = null!;
}
