using System;
using System.Linq;
using System.Threading.Tasks;
using Api.Application.Abstractions;
using Api.Domain.Entities;
using Api.Domain.Enums;
using Api.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Api.Application.Services
{
    internal sealed class RoleService : IRoleService
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<RoleService> _logger;

        public RoleService(
            AppDbContext dbContext, 
            ILogger<RoleService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<Role?> GetAsync(string? role, RoleScope scope)
        {
            if (string.IsNullOrWhiteSpace(role))
            {
                return null;
            }

            try
            {
                return await _dbContext.Roles
                    .AsNoTracking()
                    .Where(r => r.Scope == scope)
                    .FirstOrDefaultAsync(r => r.Name == role);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Role: {RoleName} in scope: {RoleScope} retrieval failed", role, scope);
                return null;
            }
        }
    }
}