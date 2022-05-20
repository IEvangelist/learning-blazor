// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Functions;

public sealed class WeatherFunctions
{
    private readonly IWeatherService _weatherService;
    private readonly ILogger<WeatherFunctions> _logger;

    public WeatherFunctions(
        IWeatherService weatherService,
        ILogger<WeatherFunctions> logger) =>
        (_weatherService, _logger) = (weatherService, logger);

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
