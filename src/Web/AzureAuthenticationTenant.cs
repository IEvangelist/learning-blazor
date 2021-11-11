// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor;

static class AzureAuthenticationTenant
{
    const string TenantHost =
        "https://learningblazor.onmicrosoft.com";

    const string TenantPublicAppId =
        "ee8868e7-73ad-41f1-88b4-dc698429c8d4";

    /// <summary>
    /// Gets a formatted string value
    /// that represents the scope URL:
    /// <c>{tenant-host}/{app-id}/User.ApiAccess</c>.
    /// </summary>
    internal const string ScopeUrl =
        $"{TenantHost}/{TenantPublicAppId}/User.ApiAccess";
}
