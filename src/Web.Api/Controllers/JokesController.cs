// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Api.Controllers;

[
    ApiController,
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
        _logger.LogInformation("{DateTime}: Getting weather", DateTime.UtcNow);

        JokeResponse joke = await _jokeFactory.GetRandomJokeAsync();

        return new JsonResult(joke, DefaultJsonSerialization.Options);
    }
}
