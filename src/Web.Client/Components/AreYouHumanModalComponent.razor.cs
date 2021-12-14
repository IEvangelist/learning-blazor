// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Components
{
    public sealed partial class AreYouHumanModalComponent
    {
        private ModalComponent _modal = null!;
        private bool? _answeredCorrectly = null!;
        private string? _attemptedAnswer;

        private string _inputValidityClass => _answeredCorrectly switch
        {
            null => "",
            false => "invalid",
            _ => ""
        };

        [Parameter]
        public AreYouHumanMath Model { get; set; }

        private async Task Show() => await _modal.Show();

        private async Task Confirm() => await _modal.Confirm();

        private void OnDismissed(DismissalReason reason) { }
            //Logger.LogInformation("User '{Reason}' the contact modal.", reason);

        private async Task AttemptToVerify()
        {
            if (int.TryParse(_attemptedAnswer, out var attemptedAnswer))
            {
                _answeredCorrectly = Model.IsCorrect(attemptedAnswer);
                if (_answeredCorrectly is true)
                {
                    await _modal.Dismiss(DismissalReason.Verified);
                }
            }
        }
    }
}
