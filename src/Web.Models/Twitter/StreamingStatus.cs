// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Models
{
    public record StreamingStatus(
        bool IsStreaming,
        string Message,
        string[] Tracks);
}
