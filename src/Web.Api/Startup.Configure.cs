// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Api;

public sealed partial class Startup
{
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
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
