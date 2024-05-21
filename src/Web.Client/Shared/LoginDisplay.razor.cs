﻿// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Shared;

public sealed partial class LoginDisplay
{
    [Inject]
    public NavigationManager Navigation { get; set; } = null!;

    void OnLogIn(MouseEventArgs args) =>
        Navigation.NavigateTo("authentication/login", true);

    void OnLogOut(MouseEventArgs args) =>
        Navigation.NavigateToLogout("authentication/logout");
}
