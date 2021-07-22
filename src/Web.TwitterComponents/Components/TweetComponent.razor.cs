// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using Learning.Blazor.Models;
using Microsoft.AspNetCore.Components;

namespace Learning.Blazor.TwitterComponents.Components
{
    public sealed partial class TweetComponent
    {
        [Parameter]
        public TweetContents Tweet { get; set; } = null!;
    }
}
