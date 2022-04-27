// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Extensions;

public static class ClaimsPrincipalExtensions
{
    /// <summary>
    /// Gets the first email address (if available) from the "emails" claim.
    /// </summary>
    /// <param name="user">The current user in context.</param>
    /// <returns>An email address if found, else <c>null</c>.</returns>
    public static string? GetFirstEmailAddress(this ClaimsPrincipal user) =>
        user.GetEmailAddresses()?.FirstOrDefault();

    /// <summary>
    /// Gets the email addresses (if available) from the "emails" claim.
    /// </summary>
    /// <param name="user">The current user in context.</param>
    /// <returns>An email address array if found, else <c>null</c>.</returns>
    public static string[]? GetEmailAddresses(this ClaimsPrincipal user)
    {
        if (user is null) return null;

        var emails = user.FindFirst("emails");
        if (emails is { ValueType: ClaimValueTypes.String } and { Value.Length: > 0 })
        {
            return emails.Value.StartsWith("[")
                ? emails.Value.FromJson<string[]>()
                : new[] { emails.Value };
        }

        return null;
    }
}
