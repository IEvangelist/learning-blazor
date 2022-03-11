// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Api.Controllers;

[
    Authorize,
    RequiredScope(new[] { "User.ApiAccess" }),
    ApiController,
    Route("api/cultures")
]
public class CulturesController : ControllerBase
{
    readonly IHttpClientFactory _httpClientFactory;
    readonly IMemoryCache _cache;
    readonly ILogger<CulturesController> _logger;

    public CulturesController(
        IHttpClientFactory httpClientFactory,
        IMemoryCache cache,
        ILogger<CulturesController> logger) =>
        (_httpClientFactory, _cache, _logger) = (httpClientFactory, cache, logger);

    [
        HttpGet,
        Route("all"),
        Produces(MediaTypeNames.Application.Json)
    ]
    public async Task<IActionResult> Get(
        [FromServices] WeatherLanguageService languageService)
    {
        var cultures = await _cache.GetOrCreateAsync(CacheKeys.AzureCultures,
            async options =>
            {
                // These rarely ever change, cache aggressively.
                options.SetSlidingExpiration(TimeSpan.FromDays(1));
                options.SetAbsoluteExpiration(TimeSpan.FromDays(3));

                using var client = _httpClientFactory.CreateClient();
                client.BaseAddress = new Uri("https://api.cognitive.microsofttranslator.com");

                var cultutes = await client.GetFromJsonAsync<AzureTranslationCultures>(
                    "languages?api-version=3.0&scope=translation",
                    DefaultJsonSerialization.Options);

                var weatherLanguages =
                    languageService.GetWeatherLanguages();

                var applicableCultures =
                    cultutes!.Translation.Where(t => weatherLanguages.Any(wl => wl.WeatherLanguageId == t.Key))
                        .ToDictionary(t => t.Key, t => t.Value);

                return cultutes! with
                {
                    Translation = applicableCultures
                };
            });

        return new JsonResult(cultures);
    }
}
