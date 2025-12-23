using System;
using System.Linq;
using System.Threading.Tasks;
using Api.Application.Abstractions;
using Api.Domain.Entities;
using Api.Domain.Enums;
using Api.Infrastructure.Cache;
using Api.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Api.Application.Services
{
    internal sealed class RoleService : IRoleService
    {
        private readonly AppDbContext _dbContext;
        private readonly ICacheProvider _cacheProvider;
        private readonly IOptions<RoleCacheOptions> _cacheOptions;
        private readonly ILogger<RoleService> _logger;

        public RoleService(
            AppDbContext dbContext, 
            ICacheProvider cacheProvider, 
            IOptions<RoleCacheOptions> cacheOptions, 
            ILogger<RoleService> logger)
        {
            _dbContext = dbContext;
            _cacheProvider = cacheProvider;
            _cacheOptions = cacheOptions;
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
                var cacheKey = RoleCacheKeys.ForNameAndScope(role, scope);
                return await _cacheProvider.ReadThroughAsync(cacheKey, _cacheOptions.Value, async () =>
                {
                    return await _dbContext.Roles
                        .AsNoTracking()
                        .Where(r => r.Scope == scope)
                        .FirstOrDefaultAsync(r => r.Name == role);
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Role: {RoleName} retrieval failed", role);
                return null;
            }
        }
    }
}