// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.Localization;

namespace Learning.Blazor.Localization
{
    internal class CoalescingStringLocalizer<T>
    {
        private readonly IStringLocalizer<T> _localizer = null!;
        private readonly IStringLocalizer<SharedResource> _sharedLocalizer = null!;

        public CoalescingStringLocalizer(
            IStringLocalizer<T> localizer,
            IStringLocalizer<SharedResource> sharedLocalizer) =>
            (_localizer, _sharedLocalizer) = (localizer, sharedLocalizer);

        /// <summary>
        /// Gets the localized content for the current subcomponent,
        /// relying on the contextually appropriate <see cref="IStringLocalizer{T}"/> implementation.
        /// </summary>
        internal LocalizedString this[string name] =>
            _localizer[name] ?? _sharedLocalizer[name] ?? new(name, name);

        /// <summary>
        /// Gets the localized content for the current subcomponent,
        /// relying on the contextually appropriate <see cref="IStringLocalizer{T}"/> implementation.
        /// </summary>
        internal LocalizedString this[string name, params object[] arguments] =>
            _localizer[name, arguments] ?? _sharedLocalizer[name, arguments] ?? new(name, name);
    }
}
