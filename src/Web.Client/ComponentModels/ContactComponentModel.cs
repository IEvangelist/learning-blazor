// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using Learning.Blazor.DataAnnotations;
using Learning.Blazor.Models;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Versioning;

namespace Learning.Blazor.ComponentModels;

public record ContactComponentModel()
{
    [Required]
    public string? FirstName { get; set; } = null!;

    [Required]
    public string? LastName { get; set; } = null!;

    [RegexEmailAddress(IsRequired = true)]
    public string? EmailAddress { get; set; } = null!;

    [Required]
    public string? Subject { get; set; } = null!;

    [RequiredAs<bool>(true)]
    public bool AgreesToTerms { get; set; }

    public AreYouHumanMath<int> NotRobot { get; } =
        AreYouHumanMath<int>.RandomIntegerFactory(
            MathOperator.Subtraction);

    [Required]
    public string? Result { get; set; }

    [Required]
    public string? Message { get; set; } = null!;

    public string RobotQuestion => NotRobot.GetQuestion();
}
