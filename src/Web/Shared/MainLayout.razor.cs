// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System.Runtime.InteropServices;
using Learning.Blazor.Services;
using Microsoft.AspNetCore.Components;

namespace Learning.Blazor.Shared;

public partial class MainLayout : IDisposable
{
    [Inject]
    public AppInMemoryState? AppState { get; set; }

    /// <inheritdoc cref="RuntimeInformation.FrameworkDescription" />
    public string? FrameworkDescription => AppState?.FrameworkDescription;

    protected override void OnInitialized()
    {
        if (AppState is { })
        {
            AppState.StateChanged += StateHasChanged;
            AppState.FrameworkDescription = RuntimeInformation.FrameworkDescription;
        }

        base.OnInitialized();
    }

#pragma warning disable CA1816 // Dispose methods should call SuppressFinalize
    // Justification: Derived types will be sealed.
    void IDisposable.Dispose() =>
#pragma warning restore CA1816 // Dispose methods should call SuppressFinalize
        AppState!.StateChanged -= StateHasChanged;
}
