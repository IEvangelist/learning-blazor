// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using Microsoft.AspNetCore.Components;

namespace Learning.Blazor.Components;

public partial class ModalComponent
{
    private string _isActiveClass => IsActive ? "is-active" : "";

    [Parameter]
    public EventCallback<DismissalReason> OnDismissed { get; set; }

    [Parameter]
    public bool IsActive { get; set; }

    [Parameter]
    public RenderFragment TitleContent { get; set; } = null!;

    [Parameter]
    public RenderFragment BodyContent { get; set; } = null!;

    [Parameter]
    public RenderFragment ButtonContent { get; set; } = null!;

    /// <summary>
    /// Gets the reason that the <see cref="ModalComponent"/> was dismissed.
    /// </summary>
    public DismissalReason Reason { get; private set; }

    public Task Dismiss(DismissalReason reason) =>
        InvokeAsync(async () =>
        {
            (IsActive, Reason) = (false, reason);

            if (OnDismissed.HasDelegate)
            {
                await OnDismissed.InvokeAsync(Reason);
            }

            StateHasChanged();
        });

    public Task Show() =>
        InvokeAsync(() =>
        {
            (IsActive, Reason) = (true, default);

            StateHasChanged();
        });

    public Task Confirm() => Dismiss(DismissalReason.Confirmed);

    public Task Cancel() => Dismiss(DismissalReason.Cancelled);
}

public enum DismissalReason
{
    Unknown,
    Confirmed,
    Cancelled
}
