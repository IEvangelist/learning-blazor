// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using Microsoft.AspNetCore.Components;

namespace Learning.Blazor.TwitterComponents.Components
{
    public sealed partial class TwitterTracksModalComponent
    {
        private string _isActiveClass => IsActive ? "is-active" : "";
        private string? _track = null;

        [Parameter]
        public bool IsActive { get; set; } = true;

        // TODO: https://github.com/dotnet/aspnetcore/blob/main/src/Components/Components/src/EditorRequiredAttribute.cs
        [Parameter/*, EditorRequired*/]
        public ISet<string> Tracks { get; set; } = null!;

        private void Dismiss() => IsActive = false;

        private void RemoveTrack(string track) => Tracks?.Remove(track);

        private void AddTrack()
        {
            if (_track is null)
            {
                return;
            }

            _ = Tracks?.Add(_track);
            _track = null;
        }
    }
}
