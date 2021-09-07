// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string[]? GetEmailAddresses(this ClaimsPrincipal user)
        {
            if (user is null) return null;

            var emails = user.FindFirst("emails");
            if (emails is { ValueType: ClaimValueTypes.String } and { Value: { Length: > 0 } })
            {
                return emails.Value.StartsWith("[")
                    ? emails.Value.FromJson<string[]>()
                    : new[] { emails.Value };
            }

            return null;
        }

        public static string? GetFirstEmailAddress(this ClaimsPrincipal user) =>
            GetEmailAddresses(user)?.FirstOrDefault();
    }
}
