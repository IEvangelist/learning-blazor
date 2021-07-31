// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Linq;
using System.Net.Mime;
using Learning.Blazor.Api.Extensions;
using Learning.Blazor.Api.Hubs;
using Learning.Blazor.Api.Localization;
using Learning.Blazor.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;

namespace Learning.Blazor.Api
{
    public class Startup
    {
        const string CorsPolicy = nameof(CorsPolicy);

        readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration) => _configuration = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddResponseCompression(
                options => options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                    new[] { MediaTypeNames.Application.Octet }));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApi(_configuration.GetSection("AzureAdB2C"));

            services.Configure<JwtBearerOptions>(
                JwtBearerDefaults.AuthenticationScheme,
                options => options.TokenValidationParameters.NameClaimType = "name");

            services.AddJokeServices(_configuration);
            services.AddApiServices(_configuration);

            services.AddCors(
                options => options.AddPolicy(
                    name: CorsPolicy,
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()));

            services.AddControllers();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Web.Api",
                    Version = "v1",
                    License = new OpenApiLicense
                    {
                        Name = "MIT License",
                        Url = new Uri("https://opensource.org/licenses/MIT")
                    }
                });
                //options.AddSecurityDefinition("ApiAccess", new OpenApiSecurityScheme
                //{
                //    Name = "auth8",
                //    Type = SecuritySchemeType.Http,
                //    Scheme = JwtBearerDefaults.AuthenticationScheme,
                //    In = ParameterLocation.Header
                //});
                //options.AddSecurityRequirement(new OpenApiSecurityRequirement
                //{
                //    {
                //        new OpenApiSecurityScheme
                //        {
                //            Reference = new OpenApiReference
                //            {
                //                Id = "ApiAccess",
                //                Type = ReferenceType.SecurityScheme
                //            }
                //        },
                //        Array.Empty<string>()
                //    }
                //});
            });

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
            app.UseCors(CorsPolicy);

            app.UseResponseCompression();

            var localizationOptions = new RequestLocalizationOptions()
                .SetDefaultCulture(Cultures.Default)
                .AddSupportedCultures(Cultures.Supported)
                .AddSupportedUICultures(Cultures.Supported);

            app.UseRequestLocalization(localizationOptions);

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<NotificationHub>("/notifications");
            });
        }
    }
}
