// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Api.Controllers;

[
    Authorize,
    RequiredScope(["User.ApiAccess"]),
    ApiController,
    Route("api/cultures")
]
public sealed class CulturesController(
    IHttpClientFactory httpClientFactory,
    IMemoryCache cache) : ControllerBase
{
    [
        HttpGet,
        Route("all"),
        Produces(MediaTypeNames.Application.Json)
    ]
    public async Task<IActionResult> Get(
        [FromServices] WeatherLanguageService languageService)
    {
        var cultures = await cache.GetOrCreateAsync(CacheKeys.AzureCultures,
            async options =>
            {
                // These rarely ever change, cache aggressively.
                options.SetSlidingExpiration(TimeSpan.FromDays(1));
                options.SetAbsoluteExpiration(TimeSpan.FromDays(3));

                using var client = httpClientFactory.CreateClient();
                client.BaseAddress = new Uri("https://api.cognitive.microsofttranslator.com");

                var cultures = await client.GetFromJsonAsync<AzureTranslationCultures>(
                    "languages?api-version=3.0&scope=translation",
                    DefaultJsonSerialization.Options);

                var weatherLanguages =
                    languageService.GetWeatherLanguages();

                var applicableCultures =
                    cultures!.Translation.Where(t => weatherLanguages.Any(wl => wl.WeatherLanguageId == t.Key))
                        .ToDictionary(t => t.Key, t => t.Value);

                return cultures! with
                {
                    Translation = applicableCultures
                };
            });

        return new JsonResult(cultures);
    }
}
