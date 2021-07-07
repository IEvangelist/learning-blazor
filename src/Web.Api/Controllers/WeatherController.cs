// Copyright (c) 2021 David Pine. All rights reserved.
//  Licensed under the MIT License.

using Learning.Blazor.Api.Requests;
using Learning.Blazor.Api.Services;
using Learning.Blazor.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web.Resource;
using System;
using System.Threading.Tasks;

namespace Learning.Blazor.Api.Controllers
{
    [
        Authorize,
        ApiController,
        Route("api/weather")
    ]
    internal class WeatherController : ControllerBase
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

        [HttpPost, Route("latest")]
        public Task<WeatherDetails?> Post(
            [FromBody] WeatherRequest request)
        {
            HttpContext.VerifyUserHasAnyAcceptedScope(s_scopeRequiredByApi);

            _logger.LogInformation("{DateTime}: Getting weather", DateTime.UtcNow);

            return _weatherFunctionClientService.GetWeatherAsync(request);
        }
    }
}
