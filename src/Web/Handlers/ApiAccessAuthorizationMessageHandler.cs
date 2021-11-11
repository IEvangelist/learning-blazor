﻿// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using Learning.Blazor.Options;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.Options;

namespace Learning.Blazor.Handlers;

public class ApiAccessAuthorizationMessageHandler : AuthorizationMessageHandler
{
    public ApiAccessAuthorizationMessageHandler(
        IAccessTokenProvider provider,
        NavigationManager navigation,
        IOptions<WebApiOptions> options) : base(provider, navigation) =>
        ConfigureHandler(
            authorizedUrls: new[]
            {
                options.Value.WebApiServerUrl,
                options.Value.PwnedWebApiServerUrl,
                "https://learningblazor.b2clogin.com"
            },
            scopes: new[] { AzureAuthenticationTenant.ScopeUrl });
}
