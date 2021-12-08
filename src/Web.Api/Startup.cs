// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Api;

public class Startup
{
    readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration) => _configuration = configuration;

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddResponseCompression(
            options =>
            {
                options.EnableForHttps = true;
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                    new[] { MediaTypeNames.Application.Octet });
            });

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApi(_configuration.GetSection("AzureAdB2C"));

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
        services.AddSwaggerGen(options => options.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "Web.Api",
            Version = "v1",
            License = new OpenApiLicense
            {
                Name = "MIT License",
                Url = new Uri("https://opensource.org/licenses/MIT")
            }
        }));

        services.AddSignalR(options => options.EnableDetailedErrors = true)
                .AddMessagePackProtocol();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v1/swagger.json", "Web.Api v1"));
        }

        app.UseHttpsRedirection();
        app.UseRouting();

        var webClientOrigin = _configuration["WebClientOrigin"];
        app.UseCors(options =>
            options.WithOrigins(
                    "https://localhost:5001", webClientOrigin)
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials());

        var localizationOptions = new RequestLocalizationOptions()
            .SetDefaultCulture(Cultures.Default)
            .AddSupportedCultures(Cultures.Supported)
            .AddSupportedUICultures(Cultures.Supported);

        app.UseRequestLocalization(localizationOptions);

        app.UseAuthentication();
        app.UseAuthorization();
        app.UseResponseCaching();
        app.UseResponseCompression();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapHub<NotificationHub>("/notifications");
        });
    }
}
