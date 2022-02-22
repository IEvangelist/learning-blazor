// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.ComponentModels;

public sealed record NotificationComponentModel(
    string Text,
    NotificationType NotificationType,
    bool IsDismissed = false)
{
    public Uri? ContextualUri { get; init; }
}

class NotificationComponentModelComparer : IEqualityComparer<NotificationComponentModel>
{
    /// <summary>
    /// Compares only three of the four properties:
    /// <see cref="NotificationComponentModel.Text"/>,
    /// <see cref="NotificationComponentModel.NotificationType"/>, and
    /// <see cref="NotificationComponentModel.ContextualUri"/>. The
    /// <see cref="NotificationComponentModel.IsDismissed"/> is a UI concern only.
    /// </summary>
    internal static IEqualityComparer<NotificationComponentModel> Instance { get; } =
        new NotificationComponentModelComparer();

    private NotificationComponentModelComparer() { }

    public bool Equals(
        NotificationComponentModel? left, NotificationComponentModel? right)
    {
        if (left is null && right is null)
        {
            return true;
        }

        if (left is not null && right is not null)
        {
            return left.Text == right.Text
                && left.NotificationType == right.NotificationType
                && left.ContextualUri == right.ContextualUri;
        }

        return false;
    }

    public int GetHashCode(
        [DisallowNull] NotificationComponentModel notificationComponentModel) =>
        HashCode.Combine(
            notificationComponentModel.Text,
            notificationComponentModel.NotificationType,
            notificationComponentModel.ContextualUri);
}
