using Xunit;

namespace Web.Client.EndToEndTests;

public class LoginToApp
{
    const string UsernameEnvVarKey = "TEST_USERNAME";
    const string PasswordEnvVarKey = "TEST_PASSWORD";

    [Fact]
    public async Task CanLoginWithVerifiedCredentials()
    {
        var username = GetEnvironmentVariable(UsernameEnvVarKey);
        var password = GetEnvironmentVariable(PasswordEnvVarKey);

        using var playwright = await Playwright.CreateAsync();
        
    }
}
