// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using Learning.Blazor.Api.Http;
using Learning.Blazor.Extensions;
using Learning.Blazor.JokeServices;
using Learning.Blazor.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web.Resource;
using System;
using System.Globalization;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Learning.Blazor.Api.Controllers
{
    [
        ApiController,
        EnableCors(CorsPolicyName.DefaultName),
        Route("api/jokes")
    ]
    public class JokesController : ControllerBase
    {
        private readonly IJokeFactory _jokeFactory;
        private readonly ILogger<JokesController> _logger;

        public JokesController(
            IJokeFactory jokeFactory,
            ILogger<JokesController> logger) =>
            (_jokeFactory, _logger) = (jokeFactory, logger);

        [
            HttpGet,
            Route("random"),
            Produces(MediaTypeNames.Application.Json)
        ]
        public async Task<IActionResult> Get()
        {
            HttpContext.VerifyUserHasAnyAcceptedScope("User.ApiAccess");

            var locale = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
            if (locale is not null)
            {

            }

            _logger.LogInformation("{DateTime}: Getting weather", DateTime.UtcNow);

            JokeResponse joke = await _jokeFactory.GetRandomJokeAsync();

            return new JsonResult(joke, DefaultJsonSerialization.Options);
        }
    }
}
