// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using Learning.Blazor.Api.Services;
using Learning.Blazor.Extensions;
using Learning.Blazor.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Learning.Blazor.Api.Controllers
{
    [
        AllowAnonymous,
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
            //HttpContext.VerifyUserHasAnyAcceptedScope(s_scopeRequiredByApi);

            _logger.LogInformation("{DateTime}: Getting weather", DateTime.UtcNow);

            var weatherDetails = await _weatherFunctionClientService.GetWeatherAsync(request);
            return new JsonResult(weatherDetails, DefaultJsonSerialization.Options);
        }
    }
}
