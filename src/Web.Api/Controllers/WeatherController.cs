// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Api.Controllers;

[
    Authorize,
    RequiredScope(["User.ApiAccess"]),
    ApiController,
    Route("api/weather")
]
public sealed class WeatherController : ControllerBase
{
    [
        HttpGet,
        Route("languages"),
        ResponseCache(
            Duration = 259_200, /* three days in seconds */
            Location = ResponseCacheLocation.Client)
    ]
    public IActionResult Languages(
        [FromServices] WeatherLanguageService languageService) =>
        new JsonResult(languageService.GetWeatherLanguages());
}
