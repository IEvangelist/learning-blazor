// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Web.Client.EndToEndTests;

public sealed partial class LoginToApp
{
    const string LearningBlazorSite = "https://webassemblyof.net";
    const string LearningBlazorB2CSite = "https://learningblazor.b2clogin.com";

    static IBrowserType ToBrowser(string browser, IPlaywright pw) =>
        browser switch { "chromium" => pw.Chromium, "firefox" => pw.Firefox, _ => pw.Webkit };

    static Credentials GetCredentials() =>
        new(
            GetEnvironmentVariable("TEST_USERNAME"),
            GetEnvironmentVariable("TEST_PASSWORD"));

    readonly record struct Credentials(string? Username, string? Password);

    [
        Theory,
        InlineData("chromium"),
        InlineData("firefox")
    ]
    public async Task CanLoginWithVerifiedCredentials(string browserName)
    {
        var (username, password) = GetCredentials();
        Assert.NotNull(username);
        Assert.NotNull(password);

        using var playwright = await Playwright.CreateAsync();
        await using var sut = await ToBrowser(browserName, playwright)
            .LaunchAsync(new()
            {
                Headless = false
            });

        await using var context = await sut.NewContextAsync(
            new()
            {
                Permissions = new[] { "geolocation" },
                Geolocation = new Geolocation() // Milwaukee, WI
                {
                    Latitude = 43.04181f,
                    Longitude = -87.90684f
                }
            });

        var loginPage = await context.NewPageAsync();
        await loginPage.RunAndWaitForNavigationAsync(async () =>
        {
            await loginPage.GotoAsync(LearningBlazorSite);
        },
        new()
        {
            UrlString = $"{LearningBlazorB2CSite}/**",
            WaitUntil = WaitUntilState.NetworkIdle
        });

        // Enter our test credentals, and attempt "sign in".
        await loginPage.FillAsync("#email", username ?? "fail");
        await loginPage.FillAsync("#password", password ?? "?!?!");
        await loginPage.ClickAsync("#next" /* "Sign in" button */);

        var selector = "#weather-city-state";
        var text = await loginPage.Locator(selector).InnerTextAsync();
        Assert.Equal("Milwaukee, Wisconsin (US)", text);
    }
}
