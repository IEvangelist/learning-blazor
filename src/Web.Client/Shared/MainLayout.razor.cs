// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System.Runtime.InteropServices;

namespace Learning.Blazor.Shared;

public sealed partial class MainLayout : IDisposable
{
    private bool _navbarBurgerClicked = false;

    [Inject]
    public AppInMemoryState AppState { get; set; } = null!;

    protected override void OnInitialized()
    {
        if (AppState is not null)
        {
            AppState.StateChanged += StateHasChanged;
            AppState.FrameworkDescription =
                RuntimeInformation.FrameworkDescription;
        }

        base.OnInitialized();
    }

    void IDisposable.Dispose()
    {
        if (AppState is not null)
        {
            AppState.StateChanged -= StateHasChanged;
        }
    }
}
