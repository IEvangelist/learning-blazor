// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Api;

public sealed partial class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddResponseCompression(
            options =>
            {
                options.EnableForHttps = true;
                options.MimeTypes =
                    ResponseCompressionDefaults.MimeTypes.Concat(
                        new[] { MediaTypeNames.Application.Octet });
            });

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApi(
                _configuration.GetSection("AzureAdB2C"));

        services.Configure<JwtBearerOptions>(
            JwtBearerDefaults.AuthenticationScheme,
            options => options.TokenValidationParameters.NameClaimType = "name");

        services.AddApiServices(_configuration);

        var webClientOrigin = _configuration["WebClientOrigin"];
        services.AddCors(
            options => options.AddDefaultPolicy(
                builder => builder.WithOrigins(
                    "https://localhost:5001", webClientOrigin)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()));

        services.AddControllers();
        services.AddSignalR(options => options.EnableDetailedErrors = true)
                .AddMessagePackProtocol();
    }
}
