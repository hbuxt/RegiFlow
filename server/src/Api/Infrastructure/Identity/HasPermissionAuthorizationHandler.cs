using System;
using System.Linq;
using System.Threading.Tasks;
using Api.Infrastructure.Cache;
using Api.Infrastructure.Extensions;
using Api.Infrastructure.Persistence.Contexts;
using Microsoft.AspNetCore.Authorization;
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
                var logger = scope.ServiceProvider.GetRequiredService<ILogger>();
                var cacheProvider = scope.ServiceProvider.GetRequiredService<ICacheProvider>();
                var cacheOptions = scope.ServiceProvider.GetRequiredService<IOptions<PermissionCacheOptions>>();
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                var cacheKey = PermissionCacheKeys.GetByNameAndUserId(requirement.Permission, userId.Value);
                var hasPermission = false;
                
                try
                {
                    var permission = await cacheProvider.ReadThroughAsync(cacheKey, cacheOptions.Value, async () =>
                    {
                        return await dbContext.Users
                            .AsNoTracking()
                            .Where(u => u.Id == userId.Value)
                            .SelectMany(u => u.UserRoles)
                            .SelectMany(ur => ur.Role!.RolePermissions)
                            .Select(rp => rp.Permission)
                            .FirstOrDefaultAsync(p => p!.Name == requirement.Permission);
                    });

                    hasPermission = permission != null;
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Failed to check permission: {Permission} for user: {UserId}", requirement.Permission, userId.Value);
                    hasPermission = false;
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