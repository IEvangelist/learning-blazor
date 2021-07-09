// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Models
{
    public record Notification<T>(
        NotificationType Type,
        T Payload) where T : notnull
    {
        public static implicit operator T(Notification<T> notification) =>
            notification.Payload;

        public static Notification<T> FromAlert(T payload) =>
            new(NotificationType.Alert, payload);
        public static Notification<T> FromChat(T payload) =>
            new(NotificationType.Chat, payload);
        public static Notification<T> FromStatus(T payload) =>
            new(NotificationType.StreamingStatus, payload);
        public static Notification<T> FromTweet(T payload) =>
            new(NotificationType.Tweet, payload);
    }
}
