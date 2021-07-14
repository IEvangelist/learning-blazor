// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using Learning.Blazor.JokeServices;
using Learning.Blazor.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace Learning.Blazor.Api.Controllers
{
    [
        //Authorize,
        ApiController,
        Route("api/jokes")
    ]
    public class JokesController : ControllerBase
    {
        private readonly IJokeFactory _jokeFactory;
        private readonly ILogger<JokesController> _logger;

        // The Web API will only accept tokens:
        //   1) for users, and
        //   2) having the "access_as_user" scope for this API
        static readonly string[] s_scopeRequiredByApi = new string[] { "access_as_user" };

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
            //HttpContext.VerifyUserHasAnyAcceptedScope(s_scopeRequiredByApi);

            _logger.LogInformation("{DateTime}: Getting weather", DateTime.UtcNow);

            JokeResponse joke = await _jokeFactory.GetRandomJokeAsync();

            return new JsonResult(joke);
        }
    }
}
