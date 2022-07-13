// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Web.Client.EndToEndTests;

public sealed partial class LoginTests
{
    private static IEnumerable<object[]> FirefoxLoginInputs
    {
        get
        {
            yield return new object[]
            {
                BrowserType.Firefox, 43.04181f, -87.90684f,
                "Milwaukee, Wisconsin (US)"
            };
            yield return new object[]
            {
                BrowserType.Firefox, 48.864716f, 2.349014f,
                "Paris, Île-de-France (FR)", "fr-FR"
            };
            yield return new object[]
            {
                BrowserType.Firefox, 20.666222f, -103.35209f,
                "Guadalajara, Jalisco (MX)", "es-MX"
            };
        }
    }
}
