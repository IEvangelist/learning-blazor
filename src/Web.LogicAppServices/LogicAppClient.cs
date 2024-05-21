// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.LogicAppServices;

public sealed class LogicAppClient(
    IOptions<LogicAppOptions> options,
    HttpClient client,
    ILogger<LogicAppClient> logger)
{
    readonly LogicAppOptions _settings = options.Value;

    public async ValueTask SendContactMessageAsync(
        string firstName,
        string lastName,
        string fromEmail,
        string subject,
        string body)
    {
        var result = await PostRequestAsync(new
        {
            firstName,
            lastName,
            fromEmail,
            subject,
            body
        },
        _settings.ContactUrl);

        logger.LogInformation(
            "Logic App Request Response: {Result}", result);
    }

    async ValueTask<string?> PostRequestAsync(object obj, string url)
    {
        try
        {
            var json = obj.ToJson()!;
            using var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(url, content);
            if (response is not null)
            {
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
        }

        return default!;
    }
}
