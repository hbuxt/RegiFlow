using System;
using System.Linq;
using System.Threading.Tasks;
using Api.Application.Abstractions;
using Api.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Api.Application.Services
{
    internal sealed class PermissionService : IPermissionService
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<PermissionService> _logger;

        public PermissionService(
            AppDbContext dbContext, 
            ILogger<PermissionService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        
        public async Task<bool> IsAuthorizedAsync(string permissionName, Guid userId)
        {
            try
            {
                var permission = await _dbContext.UserRoles
                    .AsNoTracking()
                    .Where(ur => ur.UserId == userId)
                    .SelectMany(ur => ur.Role!.RolePermissions)
                    .Select(rp => rp.Permission)
                    .FirstOrDefaultAsync(p => p!.Name == permissionName);

                return permission != null;
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
                var permission = await _dbContext.ProjectUsers
                    .AsNoTracking()
                    .Where(pu => pu.UserId == userId && pu.ProjectId == projectId)
                    .SelectMany(pu => pu.ProjectUserRoles)
                    .SelectMany(pur => pur.Role!.RolePermissions)
                    .Select(rp => rp.Permission)
                    .FirstOrDefaultAsync(p => p!.Name == permissionName);

                return permission != null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to authorize user: {UserId} in project: {Project} against permission: {PermissionName}", 
                    userId, projectId, permissionName);
                throw;
            }
        }
    }
}