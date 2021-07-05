// Copyright (c) 2021 David Pine. All rights reserved.
//  Licensed under the MIT License.

using System.Linq;
using System.Threading.Tasks;
using Learning.Blazor.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Web.Weather.Services;

namespace Web.Weather
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

        [FunctionName("weather")]
        public async Task<IActionResult> Current(
            [HttpTrigger(
                AuthorizationLevel.Function, "get",
                Route = "current/{units}")] HttpRequest req,
            Coordinates coordinates,
            string units)
        {
            _logger.LogInformation(
                "Getting weather for: {Coords} in {Units}",
                coordinates, units);

            CurrentWeather weather =
                await _weatherService.GetCurrentWeatherAsync(
                    coordinates, units);

            _logger.LogInformation(
                "Weather is {Desc} in {Loc}",
                weather?.Weather?.FirstOrDefault()?.Description,
                weather?.Name);

            return new OkObjectResult(weather);
        }
    }
}

