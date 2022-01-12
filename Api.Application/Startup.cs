using System;
using System.Collections.Generic;
using Api.Data.Context;
using Api.Domain.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

namespace Application
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            Api.CrossCutting.DependencyInjection.ConfigureServices.ConfigureDependencyService(services);
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Application", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Entre com o Token JWT",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme {
                            Reference = new OpenApiReference {
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme
                            }
                        }, new List<string>()
                    }
                });
            });

            var signConfiguration = new SignConfiguration();
            services.AddSingleton(signConfiguration);

            var tokenConfigurations = new Tokenconfiguration();
            new ConfigureFromConfigurationOptions<Tokenconfiguration>(
                Configuration.GetSection("TokenConfigurations"))
                     .Configure(tokenConfigurations);
            services.AddSingleton(tokenConfigurations);

            services.AddAuthentication(authOptions =>
                        {
                            authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                            authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                        }).AddJwtBearer(bearerOptions =>
                        {
                            var paramsValidation = bearerOptions.TokenValidationParameters;
                            paramsValidation.IssuerSigningKey = signConfiguration.Key;
                            paramsValidation.ValidAudience = tokenConfigurations.Audience;
                            paramsValidation.ValidIssuer = tokenConfigurations.Issuer;

                            // Valida a assinatura de um token recebido
                            paramsValidation.ValidateIssuerSigningKey = true;

                            // Verifica se um token recebido ainda é válido
                            paramsValidation.ValidateLifetime = true;

                            // Tempo de tolerância para a expiração de um token (utilizado
                            // caso haja problemas de sincronismo de horário entre diferentes
                            // computadores envolvidos no processo de comunicação)
                            paramsValidation.ClockSkew = TimeSpan.Zero;
                        });

            // Ativa o uso do token como forma de autorizar o acesso
            // a recursos deste projeto
            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme‌​)
                    .RequireAuthenticatedUser().Build());
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Application v1");
                c.RoutePrefix = string.Empty;
            });


            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
