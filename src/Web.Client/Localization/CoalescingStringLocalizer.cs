// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.Localization;

namespace Learning.Blazor.Localization
{
    public class CoalescingStringLocalizer<T>
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
                HashSet<string> searchedLocations = new();
                var localizedString = _localizer[name];
                if (localizedString.ResourceNotFound)
                {
                    searchedLocations.Add(localizedString.SearchedLocation!);
                    localizedString = _sharedLocalizer[name];
                }
                if (localizedString.ResourceNotFound)
                {
                    searchedLocations.Add(localizedString.SearchedLocation!);
                    localizedString = new(name, name);
                }

                LogIfNotFound(localizedString, searchedLocations);

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
                HashSet<string> searchedLocations = new();
                var localizedString = _localizer[name, arguments];
                if (localizedString.ResourceNotFound)
                {
                    searchedLocations.Add(localizedString.SearchedLocation!);
                    localizedString = _sharedLocalizer[name, arguments];
                }
                if (localizedString.ResourceNotFound)
                {
                    searchedLocations.Add(localizedString.SearchedLocation!);
                    localizedString = new(name, name);
                }

                LogIfNotFound(localizedString, searchedLocations);

                return localizedString;
            }
        }

        void LogIfNotFound(LocalizedString localizedString, ISet<string> searchedLocations)
        {
            if (localizedString.ResourceNotFound)
            {
                _logger.LogInformation(
                    "Unable to find {Name}, searched in {Location} - using {Value}.",
                    localizedString.Name,
                    string.Join(", ", searchedLocations),
                    localizedString.Value);
            }
        }
    }
}
