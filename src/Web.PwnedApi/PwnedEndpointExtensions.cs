// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.PwnedApi;

static class PwnedEndpointExtensions
{
    internal static WebApplicationBuilder AddPwnedEndpoints(this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApi(
                builder.Configuration.GetSection("AzureAdB2C"));

        builder.Services.Configure<JwtBearerOptions>(
            JwtBearerDefaults.AuthenticationScheme,
            options => options.TokenValidationParameters.NameClaimType = "name");

        builder.Services.AddPwnedServices(
            builder.Configuration.GetSection(nameof(HibpOptions)),
            HttpClientBuilderRetryPolicyExtensions.GetDefaultRetryPolicy);

        builder.Services.AddSingleton<PwnedServices>();

        var webClientOrigin = builder.Configuration["WebClientOrigin"];
        builder.Services.AddCors(
            options => options.AddDefaultPolicy(
                builder => builder.WithOrigins(
                    "https://localhost:5001", webClientOrigin)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()));

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
            options.SwaggerDoc("v1", new()
            {
                Title = "Learning.Blazor.PwnedApi",
                Version = "v1"
            }));

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

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
                options.SwaggerEndpoint(
                    "/swagger/v1/swagger.json", "Learning.Blazor.PwnedApi v1"));
        }

        app.UseHttpsRedirection();

        var webClientOrigin = app.Configuration["WebClientOrigin"];
        app.UseCors(options =>
            options.WithOrigins(
                    "https://localhost:5001", webClientOrigin)
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials());

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapBreachEndpoints();
        app.MapPwnedPasswordsEndpoints();

        return app;
    }

    internal static WebApplication MapBreachEndpoints(this WebApplication app)
    {
        // Map "have i been pwned" breaches.
        app.MapGet("api/pwned/breaches/{email}", GetBreachHeadersForAccountAsync);
        app.MapGet("api/pwned/breach/{name}", GetBreachAsync);

        return app;
    }

    internal static WebApplication MapPwnedPasswordsEndpoints(this WebApplication app)
    {
        // Map "have i been pwned" passwords.
        app.MapGet("api/pwned/passwords/{password}", GetPwnedPasswordAsync);

        return app;
    }

    [Authorize, RequiredScope("User.ApiAccess"), EnableCors]
    internal static async Task<IResult> GetBreachHeadersForAccountAsync(
        [FromRoute] string email,
        PwnedServices pwnedServices)
    {
        var breaches = await pwnedServices.GetBreachHeadersAsync(email);
        return Results.Json(breaches, DefaultJsonSerialization.Options);
    }

    [Authorize, RequiredScope("User.ApiAccess"), EnableCors]
    internal static async Task<IResult> GetBreachAsync(
        [FromRoute] string name,
        PwnedServices pwnedServices)
    {
        var breach = await pwnedServices.GetBreachDetailsAsync(name);
        return Results.Json(breach, DefaultJsonSerialization.Options);
    }

    [Authorize, RequiredScope("User.ApiAccess"), EnableCors]
    internal static async Task<IResult> GetPwnedPasswordAsync(
        [FromRoute] string password,
        IPwnedPasswordsClient pwnedPasswordsClient)
    {
        var pwnedPassword = await pwnedPasswordsClient.GetPwnedPasswordAsync(password);
        return Results.Json(pwnedPassword, DefaultJsonSerialization.Options);
    }
}
