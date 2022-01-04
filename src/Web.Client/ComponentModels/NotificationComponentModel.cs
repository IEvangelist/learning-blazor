// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.ComponentModels;

public record NotificationComponentModel(
    string Text,
    NotificationType NotificationType,
    bool IsDismissed = false);
