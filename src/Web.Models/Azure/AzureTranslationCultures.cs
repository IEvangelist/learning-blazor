// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;

namespace Learning.Blazor.Models
{
    public record AzureTranslationCultures
    {
        public IDictionary<string, AzureCulture> Translation { get; init; } = null!;
    }
}
