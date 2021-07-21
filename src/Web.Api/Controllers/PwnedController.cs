// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System.Net.Mime;
using System.Threading.Tasks;
using Learning.Blazor.Api.Services;
using Learning.Blazor.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Learning.Blazor.Api.Controllers
{
    [
        ApiController,
        Route("api/pwned")
    ]
    public class PwnedController : ControllerBase
    {
        private readonly PwnedService _pwnedService;

        public PwnedController(PwnedService pwnedService) => _pwnedService = pwnedService;

        [
            HttpGet,
            Route("breaches/{email}"),
            Produces(MediaTypeNames.Application.Json)
        ]
        public async Task<IActionResult> BreachesFor([FromRoute] string email)
        {
            var breaches = await _pwnedService.GetBreachesAsync(email);
            return new JsonResult(breaches, DefaultJsonSerialization.Options);
        }

        [
            HttpGet,
            Route("breach/{name}"),
            Produces(MediaTypeNames.Application.Json)
        ]
        public async Task<IActionResult> BreachFor([FromRoute] string name)
        {
            // TODO: cache these very aggressively
            var breach = await _pwnedService.GetBreachDetailsAsync(name);
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
