// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.PwnedApi;

static class WebApplicationBuilderExtensions
{
    internal static WebApplicationBuilder AddPwnedEndpoints(
       this WebApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        var webClientOrigin = builder.Configuration["WebClientOrigin"];
        builder.Services.AddCors(
            options =>
                options.AddDefaultPolicy(
                    policy =>
                        policy.WithOrigins(webClientOrigin, "https://localhost:5001")
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials()));

        builder.Services.AddAuthentication(
            JwtBearerDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApi(
                builder.Configuration.GetSection("AzureAdB2C"));

        builder.Services.Configure<JwtBearerOptions>(
            JwtBearerDefaults.AuthenticationScheme,
            options =>
                options.TokenValidationParameters.NameClaimType = "name");

        var hibpOptions = builder.Configuration.GetSection(nameof(HibpOptions));
        builder.Services.AddPwnedServices(
            hibpOptions,
            HttpClientBuilderRetryPolicyExtensions.GetDefaultRetryPolicy);

        if ("demo".Equals(
            hibpOptions[nameof(HibpOptions.ApiKey)],
            StringComparison.OrdinalIgnoreCase))
        {
            builder.Services.AddSingleton<IPwnedServices, NullReturningPwnedServices>();
        }
        else
        {
            builder.Services.AddSingleton<IPwnedServices, PwnedServices>();
        }

        return builder;
    }
}
