// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System.Threading.Tasks;
using Learning.Blazor.Extensions;
using Microsoft.JSInterop;

namespace Learning.Blazor.Components
{
    public partial class ThemeToggleComponent
    {
        private string _buttonClass => _isDarkTheme ? "light" : "dark";
        private string _iconClass => _isDarkTheme ? "moon" : "sun";

        private bool _isDarkTheme;

        protected override async Task OnInitializedAsync()
        {
            _isDarkTheme =
                await JavaScript.GetCurrentDarkThemePreference(
                    this, nameof(UpdateDarkThemePreference));
        }

        [JSInvokable]
        public Task UpdateDarkThemePreference(bool isDarkTheme) =>
            InvokeAsync(() =>
            {
                _isDarkTheme = isDarkTheme;

                StateHasChanged();
            });
    }
}
