// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Net.Mime;
using System.Threading.Tasks;
using Learning.Blazor.Api.Extensions;
using Learning.Blazor.Api.Services;
using Learning.Blazor.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace Learning.Blazor.Api.Controllers
{
    [
        ApiController,
        Route("api/pwned")
    ]
    public class PwnedController : ControllerBase
    {
        private readonly PwnedService _pwnedService;
        private readonly IDistributedCache _cache;

        public PwnedController(
            PwnedService pwnedService, IDistributedCache cache) =>
            (_pwnedService, _cache) = (pwnedService, cache);

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

                var breaches = await _pwnedService.GetBreachesAsync(email);
                return breaches!;
            });

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

                var breach = await _pwnedService.GetBreachDetailsAsync(name);
                return breach!;
            });
            
            return new JsonResult(breach, DefaultJsonSerialization.Options);
        }

        [
            HttpGet,
            Route("passwords/{password}"),
            Produces(MediaTypeNames.Application.Json)
        ]
        public async Task<IActionResult> PasswordsFor([FromRoute] string password)
        {
            var pwnedPassword = await _pwnedService.GetPwnedPasswordAsync(password);
            return new JsonResult(pwnedPassword, DefaultJsonSerialization.Options);
        }
    }
}
