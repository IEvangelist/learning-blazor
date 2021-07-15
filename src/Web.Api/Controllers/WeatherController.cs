// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using Learning.Blazor.Api.Services;
using Learning.Blazor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Learning.Blazor.Api.Controllers
{
    [
        //Authorize,
        ApiController,
        Route("api/weather")
    ]
    public class WeatherController : ControllerBase
    {
        private readonly WeatherFunctionClientService _weatherFunctionClientService;
        private readonly ILogger<WeatherController> _logger;

        // The Web API will only accept tokens:
        //   1) for users, and
        //   2) having the "access_as_user" scope for this API
        static readonly string[] s_scopeRequiredByApi = new string[] { "access_as_user" };

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
            return new JsonResult(weatherDetails);
        }
    }
}
