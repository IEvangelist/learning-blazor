// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Web.Client.EndToEndTests;

public sealed partial class LoginTests
{
    private static bool IsDebugging => Debugger.IsAttached;
    private static bool IsHeadless => !IsDebugging;

    public static IEnumerable<object[]> AllLoginTestInput =>
        ChromiumLoginInputs.Concat(FirefoxLoginInputs);

    [
        Theory,
        MemberData(nameof(AllLoginTestInput))
    ]
    public async Task CanLoginWithVerifiedCredentials(
        BrowserType browserType,
        float lat,
        float lon,
        string? expected,
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
                    Longitude = lon
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
