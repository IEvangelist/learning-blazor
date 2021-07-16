// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Models
{
    /// <summary>
    /// See: https://haveibeenpwned.com/api/v3#BreachModel
    /// </summary>
    public class BreachHeader
    {
        /// <summary>
        /// A Pascal-cased name representing the breach which is unique across all other breaches. This value never changes and may be used to name dependent assets (such as images) but should not be shown directly to end users (see the "Title" attribute instead).
        /// </summary>
        public string Name { get; set; } = null!;
    }
}
