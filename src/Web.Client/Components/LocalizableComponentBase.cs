// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Components
{
    public class LocalizableComponentBase<T> : ComponentBase, IDisposable
    {
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

        public ClaimsPrincipal User { get; private set; } = null!;

        [Inject]
        private CoalescingStringLocalizer<T> CoalescingStringLocalizer { get; set; } = null!;

        [CascadingParameter]
        private Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            if (AuthenticationStateTask is not null)
            {
                AuthenticationState? authState = await AuthenticationStateTask;
                if (authState is { User: { Identity: { IsAuthenticated: true } } })
                {
                    User = authState.User;
                }
            }
        }

        /// <summary>
        /// Gets the localized content for the current subcomponent,
        /// relying on the contextually appropriate <see cref="IStringLocalizer{T}"/> implementation.
        /// </summary>
        /// <remarks>
        /// Intentionally an all lowercase character variable name, without the expected "_" prefix.
        /// This is intended to be used from within HTML templates only.
        /// Example: <c>&lt;h1&gt;@Localizer["Important Title"]&lt;/h1&gt;</c>
        /// </remarks>
        internal CoalescingStringLocalizer<T> Localizer => CoalescingStringLocalizer;

        public virtual void Dispose()
        {
            if (Navigation is not null)
            {
                Navigation.LocationChanged -= OnLocationChanged;
            }

            GC.SuppressFinalize(this);
        }

        private async void OnLocationChanged(object? sender, LocationChangedEventArgs args)
        {
            if (Logger.IsEnabled(LogLevel.Information))
            {
                Logger.LogInformation(
                    "OnLocationChanged: {Location}",
                    args.Location);
            }
    
            await OnLocationChangedAsync(args);
        }

        protected virtual ValueTask OnLocationChangedAsync(LocationChangedEventArgs args) =>
            ValueTask.CompletedTask;
    }
}
