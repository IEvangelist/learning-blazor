// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Extensions.Tests;

public sealed partial class ClaimsPrincipalExtensionsTests
{
    [Fact]
    public void GetFirstEmailAddressNull()
    {
        var user = new ClaimsPrincipalMockBuilder()
            .WithClaim(
               claimType: "emails",
               claimValue: null!)
            .Build();

        Assert.Null(user.GetFirstEmailAddress());
    }    

    [Fact]
    public void GetFirstEmailAddressKeyMismatch()
    {
        var user = new ClaimsPrincipalMockBuilder()
            .WithClaim(
               claimType: "email",
               claimValue: @"[""admin@email.org"",""test@email.org""]")
            .Build();

        Assert.Null(user.GetFirstEmailAddress());
    }

    [Fact]
    public void GetFirstEmailAddressArrayString()
    {
        var user = new ClaimsPrincipalMockBuilder()
            .WithClaim(
               claimType: "emails",
               claimValue: @"[""admin@email.org"",""test@email.org""]")
            .Build();

        var expected = "admin@email.org";
        Assert.Equal(expected, user.GetFirstEmailAddress());
    }

    [Fact]
    public void GetFirstEmailAddressGetSimpleString()
    {
        var user = new ClaimsPrincipalMockBuilder()
            .WithClaim("emails", "test@email.org")
            .Build();

        var expected = "test@email.org";
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
        var user = new ClaimsPrincipalMockBuilder()
            .WithClaim(claimType, claimValue)
            .Build();

        Assert.Equal(expected, user.GetEmailAddresses());
    }
}
