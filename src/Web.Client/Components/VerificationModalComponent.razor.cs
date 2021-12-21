// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Components
{
    public sealed partial class VerificationModalComponent
    {
        private AreYouHumanMath _math = AreYouHumanMath.CreateNew();
        private ModalComponent _modal = null!;
        private bool? _answeredCorrectly = null!;
        private string? _attemptedAnswer = null!;

        [Parameter]
        public EventCallback<bool> OnVerificationAttempted { get; set; }

        public Task PromptAsync() => _modal.ShowAsync();

        private void Refresh() =>
            (_math, _attemptedAnswer) = (AreYouHumanMath.CreateNew(), null);

        private async Task OnDismissed(DismissalReason reason)
        {
            if (OnVerificationAttempted.HasDelegate)
            {
                await OnVerificationAttempted.InvokeAsync(
                    reason is DismissalReason.Verified);

                StateHasChanged();
            }
        }

        private async Task AttemptToVerify()
        {
            if (int.TryParse(_attemptedAnswer, out var attemptedAnswer))
            {
                _answeredCorrectly = _math.IsCorrect(attemptedAnswer);
                if (_answeredCorrectly is true)
                {
                    await _modal.DismissAsync(DismissalReason.Verified);
                }
            }
            else
            {
                _answeredCorrectly = false;
            }
        }
    }
}
