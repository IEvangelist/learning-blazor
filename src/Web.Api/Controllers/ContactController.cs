// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Api.Controllers;

[
    ApiController,
    Route("api/contact")
]
public sealed class ContactController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Contact(
        [FromServices] LogicAppClient client,
        [FromBody] ContactRequest request)
    {
        if (request is null)
        {
            return BadRequest();
        }

        await client.SendContactMessageAsync(
            request.FirstName,
            request.LastName,
            request.FromEmail,
            request.Subject,
            request.Body);

        return Ok();
    }
}
