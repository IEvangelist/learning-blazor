// Copyright (c) 2021 David Pine. All rights reserved.
//  Licensed under the MIT License.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Learning.Blazor.Extensions;
using Learning.Blazor.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

namespace Learning.Blazor.Components
{
    public sealed partial class AudioDescriptionComponent
    {
        private readonly IList<double> _voiceSpeeds =
            Enumerable.Range(0, 12).Select(i => (i + 1) * .25).ToList();

        private IList<SpeechSynthesisVoice> _voices = null!;
        private string _voice = "Auto";
        private double _voiceSpeed = 1;

        private ModalComponent _modal = null!;

        [Inject]
        public IJSRuntime JavaScript { get; set; } = null!;

        [Inject]
        public ILogger<AudioDescriptionComponent> Logger { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            await UpdateClientVoices(
                await JavaScript.GetClientVoices(this, nameof(UpdateClientVoices)));
        }

        [JSInvokable]
        public Task UpdateClientVoices(string voicesJson) =>
            InvokeAsync(() =>
            {
                List<SpeechSynthesisVoice>? voices =
                    voicesJson.FromJson<List<SpeechSynthesisVoice>>();
                if (voices is { Count: > 0 })
                {
                    _voices = voices;

                    StateHasChanged();
                }
            });

        private async Task Show() => await _modal.Show();
        private async Task Confirm() => await _modal.Confirm();
        private void OnDismissed(DismissalReason reason) =>
            Logger.LogWarning("User '{Reason}' the audio description modal.", reason);
    }
}
