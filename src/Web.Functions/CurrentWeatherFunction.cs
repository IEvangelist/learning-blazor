// Copyright (c) 2021 David Pine. All rights reserved.
//  Licensed under the MIT License.

using System.Linq;
using System.Threading.Tasks;
using Learning.Blazor.Models;
using Learning.Blazor.Functions.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace Learning.Blazor.Functions
{
    public class CurrentWeatherFunction
    {
        private readonly IWeatherService _weatherService;
        private readonly ILogger<CurrentWeatherFunction> _logger;

        public CurrentWeatherFunction(
            IWeatherService weatherService,
            ILogger<CurrentWeatherFunction> logger)
        {
            _weatherService = weatherService;
            _logger = logger;
        }

        [FunctionName("currentweather")]
        public async Task<IActionResult> Current(
            [HttpTrigger(
                AuthorizationLevel.Function, "get",
                Route = "currentweather/{latitude}/{longitude}/{units}")] HttpRequest req,
            decimal latitude,
            decimal longitude,
            string units)
        {
            _logger.LogInformation(
                "Getting weather for: {Lat} {Lon} in {Units}",
                latitude, longitude, units);

            CurrentWeather? weather =
                await _weatherService.GetCurrentWeatherAsync(
                    new(latitude, longitude), units ?? "imperial");

            _logger.LogInformation(
                "Weather is {Desc} in {Loc}",
                weather?.Weather?.FirstOrDefault()?.Description,
                weather?.Name);

            return new OkObjectResult(weather);
        }
    }
}
