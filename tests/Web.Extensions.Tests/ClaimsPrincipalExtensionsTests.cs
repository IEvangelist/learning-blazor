// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Extensions.Tests;

public class ClaimsPrincipalExtensionsTests
{
    [
        Theory,
        InlineData("emails", "test@email.org", "test@email.org"),
        InlineData("emails", @"[""admin@email.org"",""test@email.org""]", "admin@email.org"),
        InlineData("email", @"[""admin@email.org"",""test@email.org""]", null),
        InlineData("emails", null, null),
    ]
    public void GetFirstEmailAddressCorrectlyGetsEmailValue(
        string claimType, string claimValue, string? expected)
    {
        var user = new ClaimsPrinipalMockBuilder()
            .WithClaim(claimType, claimValue)
            .Build();

        Assert.Equal(expected, user.GetFirstEmailAddress());
    }

    [
        Theory,
        InlineData("emails", "test@email.org", new[] { "test@email.org" }),
        InlineData("emails", @"[""admin@email.org"",""test@email.org""]", new[] { "admin@email.org", "test@email.org" }),
        InlineData("email", @"[""admin@email.org"",""test@email.org""]", null),
        InlineData("emails", null, null),
    ]
    public void GetEmailAddressesCorrectlyGetsEmails(
        string claimType, string claimValue, string[]? expected)
    {
        var user = new ClaimsPrinipalMockBuilder()
            .WithClaim(claimType, claimValue)
            .Build();

        Assert.Equal(expected, user.GetEmailAddresses());
    }
}

class ClaimsPrinipalMockBuilder
{
    readonly Dictionary<string, string> _claims = new(StringComparer.OrdinalIgnoreCase);

    internal ClaimsPrinipalMockBuilder WithClaim(string claimType, string claimValue)
    {
        _claims[claimType] = claimValue ?? "";
        return this;
    }

    internal ClaimsPrincipal Build()
    {
        var claims = _claims.Select(kvp => new Claim(kvp.Key, kvp.Value));
        var identity = new ClaimsIdentity(claims, "TestIdentity");

        return new ClaimsPrincipal(identity);
    }
}
