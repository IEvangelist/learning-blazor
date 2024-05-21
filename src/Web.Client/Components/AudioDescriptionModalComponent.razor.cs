// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Components;

public sealed partial class AudioDescriptionModalComponent
{
    [Parameter, EditorRequired]
    public AudioDescriptionDetails Details { get; set; }

    [Parameter, EditorRequired]
    public string Title { get; set; } = null!;

    [Parameter, EditorRequired]
    public EventCallback<AudioDescriptionDetails> OnDetailsSaved { get; set; }

    private string _voice = null!;
    private ModalComponent _modal = null!;

    protected override void OnParametersSet() => _voice = Details.Voice;

    private void OnVoiceSpeedChange(ChangeEventArgs args) =>
        Details = Details with
        {
            VoiceSpeed = double.TryParse(
                args?.Value?.ToString() ?? "1", out var speed) ? speed : 1
        };

    internal async Task ShowAsync() => await _modal.ShowAsync();

    internal async Task ConfirmAsync()
    {
        if (OnDetailsSaved.HasDelegate)
        {
            await OnDetailsSaved.InvokeAsync(
                Details = Details with { Voice = _voice });
        }

        await _modal.ConfirmAsync();
    }
}
