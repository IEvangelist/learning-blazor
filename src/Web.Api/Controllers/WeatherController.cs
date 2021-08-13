// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Net.Mime;
using System.Threading.Tasks;
using Learning.Blazor.Api.Services;
using Learning.Blazor.Extensions;
using Learning.Blazor.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web.Resource;

namespace Learning.Blazor.Api.Controllers
{
    [
        Authorize,
        RequiredScope(new[] { "User.ApiAccess" }),
        ApiController,
        Route("api/weather")
    ]
    public class WeatherController : ControllerBase
    {
        private readonly WeatherFunctionClientService _weatherFunctionClientService;
        private readonly ILogger<WeatherController> _logger;

        public WeatherController(
            WeatherFunctionClientService weatherFunctionClientService,
            ILogger<WeatherController> logger) =>
            (_weatherFunctionClientService, _logger) =
                (weatherFunctionClientService, logger);

        [
            HttpPost,
            Route("latest"),
            Produces(MediaTypeNames.Application.Json)
        ]
        public async Task<IActionResult> Post(
            [FromBody] WeatherRequest request)
        {
            _logger.LogInformation("{DateTime}: Getting weather", DateTime.UtcNow);

            var weatherDetails = await _weatherFunctionClientService.GetWeatherAsync(request);
            return new JsonResult(weatherDetails, DefaultJsonSerialization.Options);
        }

        [
            HttpGet,
            Route("languages"),
            ResponseCache(
                Duration = 259_200, /* three days in seconds */
                Location = ResponseCacheLocation.Client)
        ]
        public IActionResult Languages([FromServices] WeatherLanguageService languageService) =>
            new JsonResult(languageService.GetWeatherLanguages());
    }
}
