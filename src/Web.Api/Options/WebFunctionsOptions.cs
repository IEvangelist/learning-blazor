// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Api.Options;

public class WebFunctionsOptions
{
    /// <summary>
    /// The format has four named route parameters:
    /// <list type="bullet">
    /// <item><c>{lang}</c> two-letter language code.</item>
    /// <item><c>{latitude}</c> positive or negative decimal.</item>
    /// <item><c>{longitude}</c> positive or negative decimal.</item>
    /// <item><c>{units}</c> "imperial", "metric", or "standard".</item>
    /// </list>
    /// </summary>
    /// <remarks>
    /// For example:
    ///   .../api/currentweather/{lang}/{latitude}/{longitude}/{units}
    ///   .../api/currentweather/en/43.09398573939293/-88.2035953150909/imperial
    ///
    /// Where <c>{lang}</c> is <c>en</c>, <c>{latitude}</c> is <c>43.09398573939293</c>
    /// <c>{latitude}</c> is <c>43.09398573939293</c>, and <c>{units}</c> is <c>imperial</c>.
    /// </remarks>
    public string WeatherFunctionUrlFormat { get; set; } = null!;
}
