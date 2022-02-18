// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.LogicAppServices;

public sealed class LogicAppClient
{
    readonly LogicAppOptions _settings;
    readonly HttpClient _client;
    readonly ILogger<LogicAppClient> _logger;

    public LogicAppClient(
        IOptions<LogicAppOptions> options, HttpClient client, ILogger<LogicAppClient> logger) =>
        (_settings, _client, _logger) = (options.Value, client, logger);

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

        _logger.LogInformation(
            "Logic App Request Response: {Result}", result);
    }

    async ValueTask<string?> PostRequestAsync(object obj, string url)
    {
        try
        {
            var json = obj.ToJson()!;
            using var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(url, content);
            if (response is not null)
            {
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
        }

        return default!;
    }
}
