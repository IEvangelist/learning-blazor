// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Functions;

public sealed class WeatherFunctions(
    IWeatherService weatherService,
    ILogger<WeatherFunctions> logger)
{
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
        logger.LogInformation(
            "Getting weather for: {Lat} {Lon} in {Units}",
            latitude, longitude, units);

        try
        {
            var weather =
                await weatherService.GetWeatherAsync(
                    new(latitude, longitude), units, lang);

            return new OkObjectResult(weather?.ToJson());
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);

            return new StatusCodeResult(500);
        }
    }
}
