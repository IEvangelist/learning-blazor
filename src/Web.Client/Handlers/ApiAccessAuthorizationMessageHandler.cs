﻿// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Handlers;

public sealed class ApiAccessAuthorizationMessageHandler : AuthorizationMessageHandler
{
    public ApiAccessAuthorizationMessageHandler(
        IAccessTokenProvider provider,
        NavigationManager navigation,
        IOptions<WebApiOptions> options) : base(provider, navigation) =>
        ConfigureHandler(
            authorizedUrls:
            [
                options.Value.WebApiServerUrl!,
                options.Value.PwnedWebApiServerUrl!,
                "https://learningblazor.b2clogin.com"
            ],
            scopes: [AzureAuthenticationTenant.ScopeUrl]);
}
