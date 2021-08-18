// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.Localization;

namespace Learning.Blazor.Localization;

internal class CoalescingStringLocalizer<T>
{
    private readonly IStringLocalizer<T> _localizer = null!;
    private readonly IStringLocalizer<SharedResource> _sharedLocalizer = null!;
    private readonly ILogger<T> _logger = null!;

    public CoalescingStringLocalizer(
        IStringLocalizer<T> localizer,
        IStringLocalizer<SharedResource> sharedLocalizer,
        ILogger<T> logger) =>
        (_localizer, _sharedLocalizer, _logger) = (localizer, sharedLocalizer, logger);

    /// <summary>
    /// Gets the localized content for the current subcomponent,
    /// relying on the contextually appropriate <see cref="IStringLocalizer{T}"/> implementation.
    /// </summary>
    internal LocalizedString this[string name]
    {
        get
        {
            var localizedString = _localizer[name]
                ?? _sharedLocalizer[name]
                ?? new(name, name);

            LogIfNotFound(localizedString);

            return localizedString;
        }
    }

    /// <summary>
    /// Gets the localized content for the current subcomponent,
    /// relying on the contextually appropriate <see cref="IStringLocalizer{T}"/> implementation.
    /// </summary>
    internal LocalizedString this[string name, params object[] arguments]
    {
        get
        {
            var localizedString = _localizer[name, arguments]
                ?? _sharedLocalizer[name, arguments]
                ?? new(name, name);

            LogIfNotFound(localizedString);

            return localizedString;
        }
    }

    void LogIfNotFound(LocalizedString localizedString)
    {
        if (localizedString is { ResourceNotFound: false })
        {
            _logger.LogInformation(
                "Unable to find {Name}, searched in {Location} - using {Value}.",
                localizedString.Name,
                localizedString.SearchedLocation,
                localizedString.Value);
        }
    }
}
