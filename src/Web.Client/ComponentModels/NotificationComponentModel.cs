// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.ComponentModels;

public record NotificationComponentModel
{
    public string Text { get; init; } = null!;

    public NotificationType NotificationType { get; init; }

    public bool IsDismissed { get; init; }
}
