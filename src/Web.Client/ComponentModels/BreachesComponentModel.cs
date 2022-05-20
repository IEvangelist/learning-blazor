// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.ComponentModels;

public sealed class BreachesComponentModel
{
    [RegexEmailAddress]
    public string? EmailAddress { get; set; } = null!;
}
