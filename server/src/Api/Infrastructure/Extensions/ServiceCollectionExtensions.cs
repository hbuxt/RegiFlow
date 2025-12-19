using System;
using Api.Infrastructure.Cache;
using Api.Infrastructure.Cors;
using Api.Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Api.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var permissionCacheSection = configuration.GetRequiredSection("CacheOptions:Permissions");
            
            var corsPolicySection = configuration.GetRequiredSection("Cors:Client");
            var corsPolicyOptions = corsPolicySection.Get<CorsPolicyOptions>();
            
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
            
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
            
            services.AddMemoryCache();
            services.AddSingleton<IAuthorizationHandler, HasPermissionAuthorizationHandler>();
            services.AddSingleton<IAuthorizationPolicyProvider, HasPermissionAuthorizationPolicyProvider>();
            services.AddScoped<ICacheProvider, CacheProvider>();
            
            return services;
        }
    }
}