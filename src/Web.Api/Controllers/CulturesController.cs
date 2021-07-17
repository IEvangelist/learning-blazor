// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Mime;
using System.Threading.Tasks;
using Learning.Blazor.Api.Extensions;
using Learning.Blazor.Extensions;
using Learning.Blazor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace Learning.Blazor.Api.Controllers
{
    [
        ApiController,
        Route("api/cultures")
    ]
    public class CulturesController : ControllerBase
    {
        readonly IHttpClientFactory _httpClientFactory;
        readonly IDistributedCache _cache;

        public CulturesController(
            IHttpClientFactory httpClientFactory,
            IDistributedCache cache) =>
            (_httpClientFactory, _cache) = (httpClientFactory, cache);

        [
            HttpGet,
            Route("all"),
            Produces(MediaTypeNames.Application.Json)
        ]
        public async Task<IActionResult> Get()
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

                    return cultutes!;
                });

            return new JsonResult(cultures);
        }
    }
}
