// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Models;

public record class ActorMessage(
    Guid Id, string Text, string UserName,
    bool IsGreeting = false, bool IsEdit = false)
    : Actor(UserName);
