// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Shared
{
    public partial class LoginDisplay
    {
        void OnLogIn(MouseEventArgs args) =>
            Navigation.NavigateTo("authentication/login", true);

        async Task OnLogOut(MouseEventArgs args)
        {
            await SignOutManager.SetSignOutState();
            Navigation.NavigateTo("authentication/logout");
        }
    }
}
