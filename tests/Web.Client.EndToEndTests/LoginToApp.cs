// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Web.Client.EndToEndTests;

public sealed partial class LoginToApp
{
    const string UsernameEnvVarKey = "TEST_USERNAME";
    const string PasswordEnvVarKey = "TEST_PASSWORD";

    const string LearningBlazorSite = "https://webassemblyof.net";

    [
        Theory,
        InlineData("chromium"),
        InlineData("firefox"),
    ]
    public async Task CanLoginWithVerifiedCredentials(string browser)
    {
        // Helper to map the browser to type.
        static IBrowserType With(
            string brwsr, IPlaywright pw) => brwsr switch
            {
                "chromium" => pw.Chromium,
                "firefox" => pw.Firefox,
                _ => pw.Webkit,
            };

        // Store these as env vars, to be used by GitHub Action
        // workflow files later that invoke a login test.
        var username = GetEnvironmentVariable(UsernameEnvVarKey);
        Assert.NotNull(username);
        var password = GetEnvironmentVariable(PasswordEnvVarKey);
        Assert.NotNull(password);

        // Create a playwright instance, and configure a browser.
        using var playwright = await Playwright.CreateAsync();
        await using var sut = await With(browser, playwright)
            .LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false
            });

        // We create a reference to a page object, and navigate
        // to the site. We then click the login button.
        var loginPage = await sut.NewPageAsync();
        await loginPage.GotoAsync(LearningBlazorSite);
        await loginPage.ClickAsync("#login-button", new PageClickOptions
        {
            Trial = true
        });

        // Enter our test credentals, and attempt "sign in".
        await loginPage.FillAsync("#email",  username ?? "fail");
        await loginPage.FillAsync("#password", password ?? "?!?!");
        await loginPage.ClickAsync("#next" /* "Sign in" button */);

        // Capture the screen.
        await loginPage.ScreenshotAsync(new PageScreenshotOptions
        {
            Path = "screenshot.png"
        });
    }
}
