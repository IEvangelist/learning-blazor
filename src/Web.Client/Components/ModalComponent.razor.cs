// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Components
{
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

        public Task DismissAsync(DismissalReason reason) =>
            InvokeAsync(async () =>
            {
                (IsActive, Reason) = (false, reason);

                if (OnDismissed.HasDelegate)
                {
                    await OnDismissed.InvokeAsync(Reason);
                }

                StateHasChanged();
            });

        public Task ShowAsync() =>
            InvokeAsync(() =>
            {
                (IsActive, Reason) = (true, default);

                StateHasChanged();
            });

        public Task ConfirmAsync() => DismissAsync(DismissalReason.Confirmed);

        public Task CancelAsync() => DismissAsync(DismissalReason.Cancelled);

        public Task VerifyAsync() => DismissAsync(DismissalReason.Verified);
    }

    public enum DismissalReason
    {
        Unknown, Confirmed, Cancelled, Verified
    };
}
