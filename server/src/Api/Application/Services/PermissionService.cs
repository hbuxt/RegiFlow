using System;
using System.Linq;
using System.Threading.Tasks;
using Api.Application.Abstractions;
using Api.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Api.Application.Services
{
    internal sealed class PermissionService : IPermissionService
    {
        private readonly AppDbContext _dbContext;

        public PermissionService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public async Task<bool> IsAuthorizedAsync(string permissionName, Guid userId)
        {
            var permission = await _dbContext.UserRoles
                .AsNoTracking()
                .Where(ur => ur.UserId == userId)
                .SelectMany(ur => ur.Role!.RolePermissions)
                .Select(rp => rp.Permission)
                .FirstOrDefaultAsync(p => p!.Name == permissionName);

            return permission != null;
        }

        public async Task<bool> IsAuthorizedAsync(string permissionName, Guid userId, Guid projectId)
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
    }
}