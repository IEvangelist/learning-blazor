// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.PwnedApi;

static class PwnedEndpointExtensions
{
    internal static WebApplicationBuilder AddPwnedEndpoints(this WebApplicationBuilder builder)
    {
        var webClientOrigin = builder.Configuration["WebClientOrigin"];
        builder.Services.AddCors(
            options => options.AddDefaultPolicy(
                builder => builder.WithOrigins(
                    "https://localhost:5001", webClientOrigin)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()));

        // Add services to the container.
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApi(
                builder.Configuration.GetSection("AzureAdB2C"));

        builder.Services.AddAuthorization();
        builder.Services.AddRequiredScopeAuthorization();

        builder.Services.Configure<JwtBearerOptions>(
            JwtBearerDefaults.AuthenticationScheme,
            options => options.TokenValidationParameters.NameClaimType = "name");

        builder.Services.AddPwnedServices(
            builder.Configuration.GetSection(nameof(HibpOptions)),
            HttpClientBuilderRetryPolicyExtensions.GetDefaultRetryPolicy);

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        return builder;
    }

    /// <summary>
    /// Maps "pwned breach data" endpoints and "pwned passwords" endpoints, with Minimal APIs.
    /// </summary>
    /// <param name="app">The current <see cref="WebApplication"/> instance to map on.</param>
    /// <returns>The given <paramref name="app"/> as a fluent API.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="app"/> is <c>null</c>.</exception>
    internal static WebApplication MapPwnedEndpoints(this WebApplication app)
    {
        ArgumentNullException.ThrowIfNull(app);

        app.MapBreachEndpoints();
        app.MapPwnedPasswordsEndpoints();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();

        return app;
    }

    internal static WebApplication MapBreachEndpoints(this WebApplication app)
    {
        // Map "have i been pwned" breaches.
        app.MapGet("api/pwned/breaches/{email}", GetBreachHeadersForAccountAsync)
            .RequireAuthorization()
            .RequireScope(app.Configuration["AzureAdB2C:Scopes"]);
        app.MapGet("api/pwned/breach/{name}", GetBreachAsync)
            .RequireAuthorization()
            .RequireScope(app.Configuration["AzureAdB2C:Scopes"]);

        return app;
    }

    internal static async Task<IResult> GetBreachHeadersForAccountAsync(
        [FromRoute] string email,
        IDistributedCache cache,
        IPwnedClient pwnedClient,
        ILoggerFactory loggerFactory)
    {
        var breaches = await cache.GetOrCreateAsync(
            $"breach:{email}",
            async options =>
            {
                options.AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1);
                return await pwnedClient.GetBreachHeadersForAccountAsync(email);
            },
            loggerFactory.CreateLogger(nameof(PwnedEndpointExtensions)));

        return Results.Json(breaches, DefaultJsonSerialization.Options);
    }

    internal static async Task<IResult> GetBreachAsync(
        [FromRoute] string name,
        IDistributedCache cache,
        IPwnedBreachesClient pwnedBreachesClient,
        ILoggerFactory loggerFactory)
    {
        var breach = await cache.GetOrCreateAsync(name, async options =>
        {
            options.AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(7);

            var breach = await pwnedBreachesClient.GetBreachAsync(name);
            return breach!;
        }, loggerFactory.CreateLogger(nameof(PwnedEndpointExtensions)));

        return Results.Json(breach, DefaultJsonSerialization.Options);
    }

    internal static WebApplication MapPwnedPasswordsEndpoints(this WebApplication app)
    {
        // Map "have i been pwned" passwords.
        app.MapGet("api/pwned/passwords/{password}", GetPwnedPasswordAsync)
            .RequireAuthorization()
            .RequireScope(app.Configuration["AzureAdB2C:Scopes"]);

        return app;
    }

    internal static async Task<IResult> GetPwnedPasswordAsync(
        [FromRoute] string password,
        IPwnedPasswordsClient pwnedPasswordsClient)
    {
        var pwnedPassword = await pwnedPasswordsClient.GetPwnedPasswordAsync(password);
        return Results.Json(pwnedPassword, DefaultJsonSerialization.Options);
    }
}
