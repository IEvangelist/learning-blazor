// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Threading.Tasks;
using Learning.Blazor.Extensions;
using Learning.Blazor.Functions.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace Learning.Blazor.Functions
{
    public class WeatherFunctions
    {
        private readonly IWeatherService _weatherService;
        private readonly ILogger<WeatherFunctions> _logger;

        public WeatherFunctions(
            IWeatherService weatherService,
            ILogger<WeatherFunctions> logger)
        {
            _weatherService = weatherService;
            _logger = logger;
        }

        [FunctionName("currentweather")]
        public async Task<IActionResult> Current(
            [HttpTrigger(
                AuthorizationLevel.Function, "get",
                Route = "currentweather/{lang}/{latitude}/{longitude}/{units}")] HttpRequest req,
            string lang,
            decimal latitude,
            decimal longitude,
            string units)
        {
            _logger.LogInformation(
                "Getting weather for: {Lat} {Lon} in {Units}",
                latitude, longitude, units);

            try
            {
                var weather =
                    await _weatherService.GetWeatherAsync(
                        new(latitude, longitude), units, lang);

                return new OkObjectResult(weather?.ToJson());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return new StatusCodeResult(500);
            }
        }
    }
}
