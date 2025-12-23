using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Application.Abstractions;
using Api.Domain.Constants;
using Api.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Api.Application.Services
{
    internal sealed class PermissionService : IPermissionService
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<PermissionService> _logger;

        private readonly Dictionary<string, Func<Guid, Guid, Task<bool>>> _policies;

        public PermissionService(
            AppDbContext dbContext, 
            ILogger<PermissionService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;

            _policies = new Dictionary<string, Func<Guid, Guid, Task<bool>>>()
            {
                [Permissions.ProjectDelete] = CanDeleteProjectAsync
            };
        }
        
        public async Task<bool> IsAuthorizedAsync(string permissionName, Guid userId)
        {
            try
            {
                return await HasPermissionAsync(permissionName, userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to authorize user: {UserId} against permission: {PermissionName}", 
                    userId, permissionName);
                throw;
            }
        }

        public async Task<bool> IsAuthorizedAsync(string permissionName, Guid userId, Guid projectId)
        {
            try
            {
                if (!await HasPermissionAsync(permissionName, userId, projectId))
                {
                    return false;
                }

                return await EvaluatePolicyAsync(permissionName, userId, projectId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to authorize user: {UserId} in project: {Project} against permission: {PermissionName}", 
                    userId, projectId, permissionName);
                throw;
            }
        }

        private async Task<bool> HasPermissionAsync(string permissionName, Guid userId)
        {
            var permission = await _dbContext.UserRoles
                .AsNoTracking()
                .Where(ur => ur.UserId == userId)
                .SelectMany(ur => ur.Role!.RolePermissions)
                .Select(rp => rp.Permission)
                .FirstOrDefaultAsync(p => p!.Name == permissionName);

            return permission != null;
        }

        private async Task<bool> HasPermissionAsync(string permissionName, Guid userId, Guid projectId)
        {
            var permission = await _dbContext.ProjectUsers
                .AsNoTracking()
                .Where(pu => pu.UserId == userId && pu.ProjectId == projectId)
                .SelectMany(pu => pu.ProjectUserRoles)
                .SelectMany(pur => pur.Role!.RolePermissions)
                .Select(rp => rp.Permission)
                .FirstOrDefaultAsync(p => p!.Name == permissionName);

            return permission != null;
        }
        
        private async Task<bool> EvaluatePolicyAsync(string permissionName, Guid userId, Guid projectId)
        {
            if (!_policies.TryGetValue(permissionName, out var policy))
            {
                return true;
            }

            return await policy(userId, projectId);
        }

        private async Task<bool> CanDeleteProjectAsync(Guid userId, Guid projectId)
        {
            var role = await _dbContext.ProjectUsers
                .AsNoTracking()
                .Where(pu => pu.UserId == userId && pu.ProjectId == projectId)
                .SelectMany(pu => pu.ProjectUserRoles)
                .FirstOrDefaultAsync(pur => pur.Role!.Name == Roles.Owner);

            return role != null;
        }
    }
}