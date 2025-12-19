using System;
using System.Net.Mime;
using System.Text;
using System.Threading.RateLimiting;
using Api.Application.Abstractions;
using Api.Domain.Constants;
using Api.Domain.Enums;
using Api.Domain.ValueObjects;
using Api.Infrastructure.Cache;
using Api.Infrastructure.Cors;
using Api.Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace Api.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var permissionCacheSection = configuration.GetRequiredSection("CacheOptions:Permissions");
            
            var jwtBearerSection = configuration.GetRequiredSection("Authentication:Jwt");
            var jwtBearerOptions = jwtBearerSection.Get<JwtBearerOptions>();
            
            var corsPolicySection = configuration.GetRequiredSection("Cors:Client");
            var corsPolicyOptions = corsPolicySection.Get<CorsPolicyOptions>();
            
            if (jwtBearerOptions == null)
            {
                throw new NullReferenceException("JWT Bearer options were not provided on startup. " +
                   "JWT Bearer options are required in order to successfully setup" +
                   "authentication and authorization for authorized endpoints.");
            }
            
            if (corsPolicyOptions == null)
            {
                throw new NullReferenceException("CORS policy options were not provided on startup. " +
                    "CORS policy options are required in order to successfully setup" +
                    "CORS policies for client-server communication.");
            }
            
            services.AddOptions<PermissionCacheOptions>()
                .Bind(permissionCacheSection)
                .ValidateDataAnnotations()
                .ValidateOnStart();
            
            services.AddOptions<JwtBearerOptions>()
                .Bind(jwtBearerSection)
                .ValidateDataAnnotations()
                .ValidateOnStart();
            
            services.AddOptions<CorsPolicyOptions>()
                .Bind(corsPolicySection)
                .ValidateDataAnnotations()
                .ValidateOnStart();
            
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
            
            services.AddCors(options =>
            {
                options.AddPolicy(CorsPolicies.RegiFlowClient, policy =>
                {
                    policy.WithOrigins(corsPolicyOptions.Origins);
                    policy.AllowAnyMethod();
                    policy.WithHeaders(corsPolicyOptions.Headers);
                    policy.AllowCredentials();
                });
            });
            
            services
                .AddAuthorization()
                .AddAuthentication()
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtBearerOptions.Secret)),
                        ValidIssuer = jwtBearerOptions.Issuer,
                        ValidAudience = jwtBearerOptions.Audience,
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ClockSkew = TimeSpan.Zero
                    };
                    options.Events = new Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerEvents()
                    {
                        OnChallenge = async (context) =>
                        {
                            context.HandleResponse();

                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            context.Response.ContentType = MediaTypeNames.Application.ProblemJson;

                            var result = Result.Failure(new Error(ErrorStatus.Unauthorized));

                            await result.ToProblemDetails().ExecuteAsync(context.HttpContext);
                        }
                    };
                });
            
            services.AddRateLimiter(options =>
            {
                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
                options.AddPolicy(RateLimitPolicies.UserTokenBucket, context =>
                {
                    var userId = context.User.GetUserId();

                    return RateLimitPartition.GetTokenBucketLimiter(
                        partitionKey: userId,
                        factory: _ => new TokenBucketRateLimiterOptions()
                        {
                            TokenLimit = 20,
                            TokensPerPeriod = 10,
                            ReplenishmentPeriod = TimeSpan.FromSeconds(10),
                            AutoReplenishment = true,
                            QueueLimit = 0,
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst
                        });
                });
                options.AddPolicy(RateLimitPolicies.IpAddressFixedWindow, context =>
                {
                    var ipAddress = context.Connection.RemoteIpAddress?.ToString() ?? "anonymous";

                    return RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: ipAddress,
                        factory: _ => new FixedWindowRateLimiterOptions()
                        {
                            PermitLimit = 20,
                            Window = TimeSpan.FromMinutes(1),
                            QueueLimit = 0,
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst
                        });
                });
                options.OnRejected = async (context, token) =>
                {
                    var result = Result.Failure(new Error(ErrorStatus.TooManyRequests));

                    await result.ToProblemDetails().ExecuteAsync(context.HttpContext);
                };
            });
            
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
            
            services.AddMemoryCache();
            services.AddSingleton<IAuthorizationMiddlewareResultHandler, AuthorizationMiddlewareResultHandler>();
            services.AddSingleton<IAuthorizationHandler, HasPermissionAuthorizationHandler>();
            services.AddSingleton<IAuthorizationPolicyProvider, HasPermissionAuthorizationPolicyProvider>();
            services.AddScoped<ICacheProvider, CacheProvider>();
            
            return services;
        }
    }
}