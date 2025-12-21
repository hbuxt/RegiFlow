using System;
using System.Linq;
using System.Threading.Tasks;
using Api.Domain.Enums;
using Api.Infrastructure.Cache;
using Api.Infrastructure.Extensions;
using Api.Infrastructure.Persistence.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Api.Infrastructure.Identity
{
    internal sealed class HasPermissionAuthorizationHandler : AuthorizationHandler<HasPermissionRequirement>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        
        public HasPermissionAuthorizationHandler(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }
        
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, HasPermissionRequirement requirement)
        {
            if (!context.User.Identity?.IsAuthenticated ?? true)
            {
                context.Fail();
                return;
            }

            var userId = context.User.GetUserId();

            if (userId == null || userId == Guid.Empty)
            {
                context.Fail();
                return;
            }

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<HasPermissionAuthorizationHandler>>();
                var cacheProvider = scope.ServiceProvider.GetRequiredService<ICacheProvider>();
                var cacheOptions = scope.ServiceProvider.GetRequiredService<IOptions<PermissionCacheOptions>>();
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                var hasPermission = false;

                if (requirement.Scope == PermissionScope.Global)
                {
                    var permissionCacheKey = PermissionCacheKeys.GetByNameAndUserId(requirement.Permission, userId.Value);
                    var permission = await cacheProvider.ReadThroughAsync(permissionCacheKey, cacheOptions.Value, async () =>
                    {
                        try
                        {
                            return await dbContext.Users
                                .AsNoTracking()
                                .Where(u => u.Id == userId.Value)
                                .SelectMany(u => u.UserRoles)
                                .SelectMany(ur => ur.Role!.RolePermissions)
                                .Select(rp => rp.Permission)
                                .FirstOrDefaultAsync(p => p!.Name == requirement.Permission);
                        }
                        catch (Exception ex)
                        {
                            logger.LogError(ex, "Failed to check permission: {PermissionName} for user: {UserId}", requirement.Permission, userId.Value);
                            return null;
                        }
                    });

                    hasPermission = permission != null;
                }
                else
                {
                    var httpContext = context.Resource as HttpContext;
                    var rawId = httpContext?.Request.RouteValues["id"]?.ToString();
                    
                    _ = Guid.TryParse(rawId, out var id);

                    var permissionCacheKey = PermissionCacheKeys.GetByNameAndUserIdInProject(requirement.Permission, userId.Value, id);
                    var permission = await cacheProvider.ReadThroughAsync(permissionCacheKey, cacheOptions.Value, async () =>
                    {
                        try
                        {
                            return await dbContext.ProjectUsers
                                .AsNoTracking()
                                .Where(pu => pu.UserId == userId.Value && pu.ProjectId == id)
                                .SelectMany(pu => pu.ProjectUserRoles)
                                .SelectMany(pur => pur.Role!.RolePermissions)
                                .Select(rp => rp.Permission)
                                .FirstOrDefaultAsync(p => p!.Name == requirement.Permission);
                        }
                        catch (Exception ex)
                        {
                            logger.LogError(ex, "Failed to check permission: {PermissionName} for user: {UserId} in project: {ProjectId}", requirement.Permission, userId.Value, id);
                            return null;
                        }
                    });
                    
                    hasPermission = permission != null;
                }
                
                if (!hasPermission)
                {
                    context.Fail();
                    return;
                }

                context.Succeed(requirement);
            }
        }
    }
}