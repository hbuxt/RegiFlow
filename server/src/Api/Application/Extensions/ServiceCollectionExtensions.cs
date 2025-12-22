using Api.Application.Abstractions;
using Api.Application.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<Program>();
            services.AddMediatR(configuration =>
            {
                configuration.RegisterServicesFromAssemblyContaining<Program>();
            });

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<IPermissionService, PermissionService>();
            
            return services;
        }
    }
}