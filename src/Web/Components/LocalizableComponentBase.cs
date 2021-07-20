// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System;
using Learning.Blazor.Localization;
using Learning.Blazor.LocalStorage;
using Learning.Blazor.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

namespace Learning.Blazor.Components
{
    public class LocalizableComponentBase<T> : ComponentBase
    {
        private readonly Lazy<FallbackLocalizer<T>> _lazyFallbackLocalizer = null!;

        public LocalizableComponentBase() =>
            _lazyFallbackLocalizer = new(() => FallbackLocalizerFactory(this));

        [Inject]
        public IStringLocalizer<SharedResource> SharedLocalizer { get; set; } = null!;

        [Inject]
        public IStringLocalizer<T> Localizer { get; set; } = null!;

        [Inject]
        public CultureService Culture { get; set; } = null!;

        [Inject]
        public ILogger<T> Logger { get; set; } = null!;

        [Inject]
        public ILocalStorage LocalStorage { get; set; } = null!;

        [Inject]
        public IJSRuntime JavaScript { get; set; } = null!;

        /// <summary>
        /// Gets the localized content for the current subcomponent,
        /// relying on the contextually appropriate <see cref="IStringLocalizer{T}"/> implementation.
        /// </summary>
        /// <remarks>
        /// Intentionally an all lowercase character variable name, without the expected "_" prefix.
        /// This is intended to be used from within HTML templates only.
        /// Example: <c>&lt;h1&gt;@localize["Important Title"]&lt;/h1&gt;</c>
        /// </remarks>
        protected FallbackLocalizer<T> localize => _lazyFallbackLocalizer.Value;

        private static FallbackLocalizer<T> FallbackLocalizerFactory(
            LocalizableComponentBase<T> @this) =>
            new(@this.Localizer, @this.SharedLocalizer);

        protected class FallbackLocalizer<TComponent>
        {
            private readonly IStringLocalizer<T> _localizer = null!;
            private readonly IStringLocalizer<SharedResource> _sharedLocalizer = null!;

            public FallbackLocalizer(
                IStringLocalizer<T> localizer,
                IStringLocalizer<SharedResource> sharedLocalizer) =>
                (_localizer, _sharedLocalizer) = (localizer, sharedLocalizer);

            /// <summary>
            /// Gets the localized content for the current subcomponent,
            /// relying on the contextually appropriate <see cref="IStringLocalizer{T}"/> implementation.
            /// </summary>
            internal LocalizedString this[string name] =>
                _localizer[name] ?? _sharedLocalizer[name] ?? new(name, name);
        }
    }
}
