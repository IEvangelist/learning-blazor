// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Pages
{
    public sealed partial class Pwned
    {
        [Inject]
        public NavigationManager Navigation { get; set; } = null!;

        private void NavigateToBreaches() => Navigation.NavigateTo("pwned/breaches");
        private void NavigateToPasswords() => Navigation.NavigateTo("pwned/passwords");
    }
}
