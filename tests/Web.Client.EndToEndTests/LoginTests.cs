// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Web.Client.EndToEndTests;

public sealed partial class LoginTests
{
    private static bool IsDebugging => Debugger.IsAttached;
    private static bool IsHeadless => !IsDebugging;

    [
        Theory,
        InlineData(
            BrowserType.Chromium, 43.04181f, -87.90684f,
            "Milwaukee, Wisconsin (US)"),
        InlineData(
            BrowserType.Chromium, 48.864716f, 2.349014f,
            "Paris, Île-de-France (FR)", "fr-FR",
            Skip = "Wait for app deployment to fix bug."),
        InlineData(
            BrowserType.Firefox, 43.04181f, -87.90684f,
            "Milwaukee, Wisconsin (US)")
    ]
    public async Task CanLoginWithVerifiedCredentials(
        BrowserType browserType,
        float lat = 43.04181f,
        float lng = -87.90684f,
        string? expected = null,
        string? locale = null)
    {
        var (username, password) = GetTestCredentials();
        
        using var playwright = await Playwright.CreateAsync();
        await using var browser = await ToBrowser(browserType, playwright)
            .LaunchAsync(new() { Headless = IsHeadless });

        await using var context = await browser.NewContextAsync(
            new()
            {
                Permissions = new[] { "geolocation" },
                Geolocation = new Geolocation()
                {
                    Latitude = lat,
                    Longitude = lng
                }
            });

        var loginPage = await context.NewPageAsync();
        await loginPage.RunAndWaitForNavigationAsync(
            async () =>
            {
                await loginPage.GotoAsync(LearningBlazorSite);
                if (locale is not null)
                {
                    await loginPage.AddInitScriptAsync(@"(locale => {
    if (locale) {
        window.localStorage.setItem(
            'client-culture-preference', `""${locale}""`);
    }
})('" + locale + "')");
                }
            },
            new()
            {
                UrlString = $"{LearningBlazorB2CSite}/**",
                WaitUntil = WaitUntilState.NetworkIdle
            });

        // Enter the test credentals, and "sign in".
        await loginPage.FillAsync("#email", username ?? "fail");
        await loginPage.FillAsync("#password", password ?? "?!?!");
        await loginPage.ClickAsync("#next" /* "Sign in" button */);

        if (IsDebugging)
        {
            loginPage.SetDefaultTimeout(0);
        }

        // Ensure the real weather data loads.
        var text = await loginPage.Locator("#weather-city-state")
            .InnerTextAsync();

        Assert.Equal(expected, text);
    }
}
