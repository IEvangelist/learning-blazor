// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using Learning.Blazor.Api.Hubs;
using Learning.Blazor.Api.Services;
using Learning.Blazor.Extensions;
using Learning.Blazor.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web.Resource;
using System;
using System.Net.Mime;
using System.Threading.Tasks;

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
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly ILogger<WeatherController> _logger;

        public WeatherController(
            WeatherFunctionClientService weatherFunctionClientService,
            IHubContext<NotificationHub> hubContext,
            ILogger<WeatherController> logger) =>
            (_weatherFunctionClientService, _hubContext, _logger) =
                (weatherFunctionClientService, hubContext, logger);

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
            if (weatherDetails is { Alerts: { Count: > 0 } })
            {
                //await _hubContext.Clients.
            }

            return new JsonResult(weatherDetails, DefaultJsonSerialization.Options);
        }

        [HttpGet, Route("languages")]
        public IActionResult Languages() =>
            new JsonResult(new WeatherLanguage[]
            {
                new("af","Afrikaans"),
                new("al", "Albanian"),
                new("ar", "Arabic"),
                new("az", "Azerbaijani"),
                new("bg", "Bulgarian"),
                new("ca", "Catalan"),
                new("cz", "Czech"),
                new("da", "Danish"),
                new("de", "German"),
                new("el", "Greek"),
                new("en", "English"),
                new("eu", "Basque"),
                new("fa", "Persian (Farsi)"),
                new("fi", "Finnish"),
                new("fr", "French"),
                new("gl", "Galician"),
                new("he", "Hebrew"),
                new("hi", "Hindi"),
                new("hr", "Croatian"),
                new("hu", "Hungarian"),
                new("id", "Indonesian"),
                new("it", "Italian"),
                new("ja", "Japanese"),
                new("kr", "Korean"),
                new("la", "Latvian"),
                new("lt", "Lithuanian"),
                new("mk", "Macedonian"),
                new("no", "Norwegian"),
                new("nl", "Dutch"),
                new("pl", "Polish"),
                new("pt", "Portuguese"),
                new("pt_br", "Português Brasil"),
                new("ro", "Romanian"),
                new("ru", "Russian"),
                new("se", "Swedish"),
                new("sk", "Slovak"),
                new("sl", "Slovenian"),
                new("es", "Spanish"),
                new("sr", "Serbian"),
                new("th", "Thai"),
                new("tr", "Turkish"),
                new("ua", "Ukrainian"),
                new("vi", "Vietnamese"),
                new("zh_cn", "Chinese Simplified"),
                new("zh_tw", "Chinese Traditional"),
                new("zu", "Zulu")
            });
    }
}
