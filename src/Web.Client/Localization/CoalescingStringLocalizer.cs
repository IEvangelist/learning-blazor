// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Localization;

public sealed class CoalescingStringLocalizer<T>(
    IStringLocalizer<T> localizer,
    IStringLocalizer<SharedResource> sharedLocalizer)
{
    /// <summary>
    /// Gets the localized content for the current sub-component,
    /// relying on the contextually appropriate
    /// <see cref="IStringLocalizer{T}"/> implementation.
    /// </summary>
    internal LocalizedString this[string name]
        => localizer[name]
        ?? sharedLocalizer[name]
        ?? new(name, name, false);

    /// <summary>
    /// Gets the localized content for the current sub-component,
    /// relying on the contextually appropriate
    /// <see cref="IStringLocalizer{T}"/> implementation.
    /// </summary>
    internal LocalizedString this[string name, params object[] arguments]
        => localizer[name, arguments]
        ?? sharedLocalizer[name, arguments]
        ?? new(name, name, false);
}
