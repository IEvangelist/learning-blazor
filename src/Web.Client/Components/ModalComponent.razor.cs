// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Components;

public partial class ModalComponent
{
    private string _isActiveClass => IsActive ? "is-active" : "";

    [Parameter, EditorRequired]
    public EventCallback<DismissalReason> OnDismissed { get; set; }

    [Parameter]
    public bool IsActive { get; set; }

    [Parameter, EditorRequired]
    public RenderFragment TitleContent { get; set; } = null!;

    [Parameter, EditorRequired]
    public RenderFragment BodyContent { get; set; } = null!;

    [Parameter, EditorRequired]
    public RenderFragment ButtonContent { get; set; } = null!;

    /// <summary>
    /// Gets the reason that the <see cref="ModalComponent"/> was dismissed.
    /// </summary>
    public DismissalReason Reason { get; private set; }

    /// <summary>
    /// Sets the <see cref="ModalComponent"/> instance's
    /// <see cref="IsActive"/> value to <c>true</c> and
    /// <see cref="Reason"/> value as <c>default</c>.
    /// It then signals for a change of state, this rerender will
    /// show the modal.
    /// </summary>
    public Task ShowAsync() =>
        InvokeAsync(() => (IsActive, Reason) = (true, default));

    /// <summary>
    /// Sets the <see cref="ModalComponent"/> instance's
    /// <see cref="IsActive"/> value to <c>false</c> and
    /// <see cref="Reason"/> value as given <paramref name="reason"/>
    /// value. It then signals for a change of state,
    /// this rerender will cause the modal to be dismissed.
    /// </summary>
    public Task DismissAsync(DismissalReason reason) =>
        InvokeAsync(async () =>
        {
            (IsActive, Reason) = (false, reason);
            if (OnDismissed.HasDelegate)
            {
                await OnDismissed.InvokeAsync(Reason);
            }
        });

    /// <summary>
    /// Dismisses the shown modal, the <see cref="Reason"/>
    /// will be set to <see cref="DismissalReason.Confirmed"/>.
    /// </summary>
    public Task ConfirmAsync() => DismissAsync(DismissalReason.Confirmed);

    /// <summary>
    /// Dismisses the shown modal, the <see cref="Reason"/>
    /// will be set to <see cref="DismissalReason.Cancelled"/>.
    /// </summary>
    public Task CancelAsync() => DismissAsync(DismissalReason.Cancelled);

    /// <summary>
    /// Dismisses the shown modal, the <see cref="Reason"/>
    /// will be set to <see cref="DismissalReason.Verified"/>.
    /// </summary>
    public Task VerifyAsync() => DismissAsync(DismissalReason.Verified);
}

public enum DismissalReason
{
    Unknown, Confirmed, Cancelled, Verified
};
