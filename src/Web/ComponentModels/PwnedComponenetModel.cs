// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System.ComponentModel.DataAnnotations;
using Learning.Blazor.Pages;

namespace Learning.Blazor.ComponentModels
{
    public class PwnedComponenetModel
    {
        [
            EmailAddress(
                ErrorMessageResourceType = typeof(Pwned),
                ErrorMessageResourceName = "InvalidEmailAddress")
        ]
        public string? EmailAddress { get; set; } = null!;

        public string? PlainTextPassword { get; set; } = null!;
    }
}
