// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Components;

public sealed partial class VerificationModalComponent
{
    private AreYouHumanMath _math = AreYouHumanMath.CreateNew();
    private ModalComponent _modal = null!;
    private bool? _answeredCorrectly = null!;
    private string? _attemptedAnswer = null!;
    private object? _state = null;

    [Parameter, EditorRequired]
    public EventCallback<(bool IsVerified, object? State)> VerificationAttempted { get; set; }

    public Task PromptAsync(object? state = null)
    {
        _state = state;
        return _modal.ShowAsync();
    }

    private void Refresh() =>
        (_math, _attemptedAnswer) = (AreYouHumanMath.CreateNew(), null);

    private Task OnDismissed(DismissalReason reason) =>
        VerificationAttempted.TryInvokeAsync
            <(bool IsVerified, object? State), VerificationModalComponent>(
            args: (reason is DismissalReason.Verified, _state),
            componentBase: null);

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
