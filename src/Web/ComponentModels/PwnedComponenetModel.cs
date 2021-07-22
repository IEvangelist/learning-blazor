// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using Learning.Blazor.DataAnnotations;

namespace Learning.Blazor.ComponentModels
{
    public class PwnedComponenetModel
    {
        [RegexEmailAddress]
        public string? EmailAddress { get; set; } = null!;

        public string? PlainTextPassword { get; set; } = null!;
    }
}
