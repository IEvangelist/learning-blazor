// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using Learning.Blazor.Localization;
using Learning.Blazor.LocalStorage;
using Learning.Blazor.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;

namespace Learning.Blazor.Components
{
    public class LocalizableComponentBase<T> : ComponentBase, IDisposable
    {
        [Inject]
        private CoalescingStringLocalizer<T> CoalescingStringLocalizer { get; set; } = null!;

        [Inject]
        public NavigationManager Navigation { get; set; } = null!;

        [Inject]
        public CultureService Culture { get; set; } = null!;

        [Inject]
        public ILogger<T> Logger { get; set; } = null!;

        [Inject]
        public ILocalStorage LocalStorage { get; set; } = null!;

        [Inject]
        public IJSRuntime JavaScript { get; set; } = null!;

        [Inject]
        public AppInMemoryState AppState { get; set; } = null!;

        /// <summary>
        /// Gets the localized content for the current subcomponent,
        /// relying on the contextually appropriate <see cref="IStringLocalizer{T}"/> implementation.
        /// </summary>
        /// <remarks>
        /// Intentionally an all lowercase character variable name, without the expected "_" prefix.
        /// This is intended to be used from within HTML templates only.
        /// Example: <c>&lt;h1&gt;@localize["Important Title"]&lt;/h1&gt;</c>
        /// </remarks>
        internal CoalescingStringLocalizer<T> localize => CoalescingStringLocalizer;

        public virtual void Dispose()
        {
            if (Navigation is not null)
            {
                Navigation.LocationChanged -= OnLocationChanged;
            }

            GC.SuppressFinalize(this);
        }

        private async void OnLocationChanged(object? sender, LocationChangedEventArgs args) =>
            await OnLocationChangedAsync(args);

        protected virtual ValueTask OnLocationChangedAsync(LocationChangedEventArgs args) =>
            ValueTask.CompletedTask;
    }
}
