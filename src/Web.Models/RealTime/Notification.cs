// Copyright (c) 2021 David Pine. All rights reserved.
//  Licensed under the MIT License.

namespace Learning.Blazor.Models
{
    public record Notification<T>(
        NotificationType Type,
        T Payload) where T : notnull
    {
        public static implicit operator T(Notification<T> notification) =>
            notification.Payload;
    }
}
