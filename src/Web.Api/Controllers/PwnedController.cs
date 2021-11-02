// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System.Net.Mime;
using HaveIBeenPwned.Client;
using Learning.Blazor.Api.Extensions;
using Learning.Blazor.DistributedCache.Extensions;
using Learning.Blazor.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Identity.Web.Resource;

namespace Learning.Blazor.Api.Controllers;

[
    Authorize,
    RequiredScope(new[] { "User.ApiAccess" }),
    ApiController,
    Route("api/pwned")
]
public class PwnedController : ControllerBase
{
    private readonly IPwnedClient _pwnedClient;
    private readonly IDistributedCache _cache;
    private readonly ILogger<PwnedController> _logger;

    public PwnedController(
        IPwnedClient pwnedClient,
        IDistributedCache cache,
        ILogger<PwnedController> logger) =>
        (_pwnedClient, _cache, _logger) = (pwnedClient, cache, logger);

    [
        HttpGet,
        Route("breaches/{email}"),
        Produces(MediaTypeNames.Application.Json)
    ]
    public async Task<IActionResult> BreachesFor([FromRoute] string email)
    {
        var breaches = await _cache.GetOrCreateAsync($"breach:{email}", async options =>
        {
            options.AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1);

            var breaches = await _pwnedClient.GetBreachHeadersForAccountAsync(email);
            return breaches!;
        }, _logger);

        return new JsonResult(breaches, DefaultJsonSerialization.Options);
    }

    [
        HttpGet,
        Route("breach/{name}"),
        Produces(MediaTypeNames.Application.Json)
    ]
    public async Task<IActionResult> BreachFor([FromRoute] string name)
    {
        var breach = await _cache.GetOrCreateAsync(name, async options =>
        {
            options.AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(7);

            var breach = await _pwnedClient.GetBreachAsync(name);
            return breach!;
        }, _logger);

        return new JsonResult(breach, DefaultJsonSerialization.Options);
    }

    [
        HttpGet,
        Route("passwords/{password}"),
        Produces(MediaTypeNames.Application.Json)
    ]
    public async Task<IActionResult> PasswordsFor([FromRoute] string password)
    {
        var pwnedPassword = await _pwnedClient.GetPwnedPasswordAsync(password);
        return new JsonResult(pwnedPassword, DefaultJsonSerialization.Options);
    }
}
