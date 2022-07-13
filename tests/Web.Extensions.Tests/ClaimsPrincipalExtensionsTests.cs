// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Extensions.Tests;

public sealed partial class ClaimsPrincipalExtensionsTests
{
    [Fact]
    public void GetFirstEmailAddressNull()
    {
        var sut = new ClaimsPrincipalBuilder()
            .WithClaim(
               claimType: "emails",
               claimValue: null!)
            .Build();

        var actual = sut.GetFirstEmailAddress();
        Assert.Null(actual);
    }    

    [Fact]
    public void GetFirstEmailAddressKeyMismatch()
    {
        var sut = new ClaimsPrincipalBuilder()
            .WithClaim(
               claimType: "email",
               claimValue: @"[""admin@email.org"",""test@email.org""]")
            .Build();

        var actual = sut.GetFirstEmailAddress();
        Assert.Null(actual);
    }

    [Fact]
    public void GetFirstEmailAddressArrayString()
    {
        var sut = new ClaimsPrincipalBuilder()
            .WithClaim(
               claimType: "emails",
               claimValue: @"[""admin@email.org"",""test@email.org""]")
            .Build();

        var expected = "admin@email.org";
        var actual = sut.GetFirstEmailAddress();
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetFirstEmailAddressGetSimpleString()
    {
        var sut = new ClaimsPrincipalBuilder()
            .WithClaim("emails", "test@email.org")
            .Build();

        var expected = "test@email.org";
        var actual = sut.GetFirstEmailAddress();
        Assert.Equal(expected, actual);
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
        var sut = new ClaimsPrincipalBuilder()
            .WithClaim(claimType, claimValue)
            .Build();

        var actual = sut.GetEmailAddresses();
        Assert.Equal(expected, actual);
    }
}
