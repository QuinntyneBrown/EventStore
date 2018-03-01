using EventStore.Infrastructure.Configuration;
using EventStore.Infrastructure.Data;
using EventStore.Infrastructure.Filters;
using EventStore.Infrastructure.Identity;
using EventStore.Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;

namespace EventStore.Web.Services
{
    public static class ServiceCollectionExtensions
    {

        public static IServiceCollection AddCustomCache(this IServiceCollection services)
        {
            services.AddMemoryCache();

            services.TryAddSingleton<ICache, MemoryCache>();

            return services;
        }

        public static IServiceCollection AddCustomConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AuthenticationSettings>(configuration.GetSection("Authentication"));            
            return services;
        }

        public static IServiceCollection AddCustomMediator(this IServiceCollection services)
        {
            services.AddMediatR(typeof(Startup));
            services.AddScoped<IMediator, EventStoreMediator>();
            return services;
        }

        public static IServiceCollection AddCustomMvc(this IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(HttpGlobalExceptionFilter));
            }).AddControllersAsServices();

            return services;
        }

        public static IServiceCollection AddCustomSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.DescribeAllEnumsAsStrings();
                options.SwaggerDoc("v1", new Info
                {
                    Title = "CQRS Example",
                    Version = "v1",
                    Description = "CQRS Example .NET Core REST API",
                });
                options.CustomSchemaIds(x => x.FullName);
            });

            return services;
        }

        public static IServiceCollection AddDataStores(this IServiceCollection services,
                                               string connectionString)
        {
            services.AddDbContextPool<EventStoreContext>(options =>
            {
                options.UseSqlServer(connectionString, b=> b.MigrationsAssembly("EventStore.Infrastructure"));
            });

            services.AddScoped<IEventStoreContext, EventStoreContext>();

            return services;
        }

        public static IServiceCollection AddSecurity(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors(options => options.AddPolicy("CorsPolicy",
                builder => builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()));

            
            services.AddHttpContextAccessor();

            
            services.TryAddSingleton<ITokenProvider, TokenProvider>();

            services.AddSingleton<IEncryptionService, EncryptionService>();

            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler
            {
                InboundClaimTypeMap = new Dictionary<string, string>()
            };

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.SecurityTokenValidators.Clear();
                    options.SecurityTokenValidators.Add(jwtSecurityTokenHandler);
                    options.TokenValidationParameters = GetTokenValidationParameters(configuration);
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            if ((context.Request.Path.Value.StartsWith("/appHub"))
                                && context.Request.Query.TryGetValue("token", out StringValues token)
                            )
                            {
                                context.Token = token;
                            }

                            return Task.CompletedTask;
                        },
                        OnAuthenticationFailed = context =>
                        {
                            var timeoutException = context.Exception;
                            return Task.CompletedTask;
                        }
                    };
                });

            return services;
        }

        private static TokenValidationParameters GetTokenValidationParameters(IConfiguration configuration)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["Authentication:JwtKey"])),
                ValidateIssuer = true,
                ValidIssuer = configuration["Authentication:JwtIssuer"],
                ValidateAudience = true,
                ValidAudience = configuration["Authentication:JwtAudience"],
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                NameClaimType = JwtRegisteredClaimNames.UniqueName
            };

            return tokenValidationParameters;
        }
    }
}
