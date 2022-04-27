// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Web.Client.EndToEndTests;

public sealed partial class LoginTests
{
    [
        Theory,
        InlineData(BrowserType.Chromium),
        InlineData(BrowserType.Firefox)
    ]
    public async Task CanLoginWithVerifiedCredentials(BrowserType browserType)
    {
        var (username, password) = GetTestCredentials();
        
        using var playwright = await Playwright.CreateAsync();
        await using var browser = await ToBrowser(browserType, playwright)
            .LaunchAsync();

        await using var context = await browser.NewContextAsync(
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
        await loginPage.RunAndWaitForNavigationAsync(
            () => loginPage.GotoAsync(LearningBlazorSite),
            new()
            {
                UrlString = $"{LearningBlazorB2CSite}/**",
                WaitUntil = WaitUntilState.NetworkIdle
            });

        // Enter the test credentals, and "sign in".
        await loginPage.FillAsync("#email", username ?? "fail");
        await loginPage.FillAsync("#password", password ?? "?!?!");
        await loginPage.ClickAsync("#next" /* "Sign in" button */);

        // Ensure the real weather data loads.
        var text = await loginPage.Locator("#weather-city-state")
            .InnerTextAsync();

        Assert.Equal("Milwaukee, Wisconsin (US)", text);
    }
}
