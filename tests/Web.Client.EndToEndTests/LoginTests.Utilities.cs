// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Web.Client.EndToEndTests;

public sealed partial class LoginTests
{
    const string LearningBlazorSite = "https://webassemblyof.net";
    const string LearningBlazorB2CSite = "https://learningblazor.b2clogin.com";

    static IBrowserType ToBrowser(BrowserType browser, IPlaywright pw) =>
        browser switch
        {
            BrowserType.Chromium => pw.Chromium,
            BrowserType.Firefox => pw.Firefox,
            _ => throw new ArgumentException($"Unknown browser: {browser}")
        };

    static Credentials GetTestCredentials()
    {
        var credentials = TryGetTestCredentials();
        Assert.NotNull(credentials.Username);
        Assert.NotNull(credentials.Password);

        return credentials;
    }

    static Credentials TryGetTestCredentials() =>
        new(
            GetEnvironmentVariable("TEST_USERNAME"),
            GetEnvironmentVariable("TEST_PASSWORD"));

    readonly record struct Credentials(
        string? Username,
        string? Password);

    public enum BrowserType
    {
        Unknown,
        Chromium,
        Firefox
    }
}
