// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace Learning.Blazor.Handlers;

public class ApiAccessAuthorizationMessageHandler : AuthorizationMessageHandler
{
    const string Scope =
        "https://learningblazor.onmicrosoft.com/ee8868e7-73ad-41f1-88b4-dc698429c8d4/User.ApiAccess";

    public ApiAccessAuthorizationMessageHandler(
        IAccessTokenProvider provider,
        NavigationManager navigation,
        IConfiguration configuration) : base(provider, navigation) =>
        ConfigureHandler(
            authorizedUrls: new[]
            {
                    configuration.GetValue<string>("WebApiServerUrl"),
                    "https://learningblazor.b2clogin.com"
            },
            scopes: new[] { Scope });
}
