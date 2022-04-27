// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Extensions.Tests;

public sealed partial class ClaimsPrincipalExtensionsTests
{
    class ClaimsPrincipalMockBuilder
    {
        readonly Dictionary<string, string> _claims = new(StringComparer.OrdinalIgnoreCase);

        internal ClaimsPrincipalMockBuilder WithClaim(
            string claimType, string claimValue)
        {
            _claims[claimType] = claimValue ?? "";
            return this;
        }

        internal ClaimsPrincipal Build()
        {
            var claims = _claims.Select(
                kvp => new Claim(kvp.Key, kvp.Value));
            var identity = new ClaimsIdentity(claims, "TestIdentity");

            return new ClaimsPrincipal(identity);
        }
    }
}
