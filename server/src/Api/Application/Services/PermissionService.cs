using System;
using System.Linq;
using System.Threading.Tasks;
using Api.Application.Abstractions;
using Api.Infrastructure.Cache;
using Api.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Api.Application.Services
{
    internal sealed class PermissionService : IPermissionService
    {
        private readonly AppDbContext _dbContext;
        private readonly ICacheProvider _cacheProvider;
        private readonly IOptions<PermissionCacheOptions> _cacheOptions;
        private readonly ILogger<PermissionService> _logger;

        public PermissionService(
            AppDbContext dbContext, 
            ICacheProvider cacheProvider, 
            IOptions<PermissionCacheOptions> cacheOptions, 
            ILogger<PermissionService> logger)
        {
            _dbContext = dbContext;
            _cacheProvider = cacheProvider;
            _cacheOptions = cacheOptions;
            _logger = logger;
        }

        public async Task<bool> IsAuthorizedAsync(string permissionName, Guid userId)
        {
            try
            {
                var cacheKey = PermissionCacheKeys.GetByNameAndUserId(permissionName, userId);
                var permission = await _cacheProvider.ReadThroughAsync(cacheKey, _cacheOptions.Value, async () =>
                {
                    return await _dbContext.Users
                        .AsNoTracking()
                        .Where(u => u.Id == userId)
                        .SelectMany(u => u.UserRoles)
                        .SelectMany(ur => ur.Role!.RolePermissions)
                        .Select(rp => rp.Permission)
                        .FirstOrDefaultAsync(p => p!.Name == permissionName);
                });

                return permission != null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to check permission: {PermissionName} for user: {UserId}", 
                    permissionName, userId);
                throw;
            }
        }

        public async Task<bool> IsAuthorizedAsync(string permissionName, Guid userId, Guid projectId)
        {
            try
            {
                var cacheKey = PermissionCacheKeys.GetByNameAndUserIdInProject(permissionName, userId, projectId);
                var permission = await _cacheProvider.ReadThroughAsync(cacheKey, _cacheOptions.Value, async () =>
                {
                    return await _dbContext.ProjectUsers
                        .AsNoTracking()
                        .Where(pu => pu.UserId == userId && pu.ProjectId == projectId)
                        .SelectMany(pu => pu.ProjectUserRoles)
                        .SelectMany(pur => pur.Role!.RolePermissions)
                        .Select(rp => rp.Permission)
                        .FirstOrDefaultAsync(p => p!.Name == permissionName);
                });

                return permission != null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to check permission: {PermissionName} in project: {ProjectId} for user: {UserId}", 
                    permissionName, userId, projectId);
                throw;
            }
        }
    }
}