using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Application.Abstractions;
using Api.Domain.Entities;
using Api.Infrastructure.Cache;
using Api.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Api.Application.Services
{
    internal sealed class UserService : IUserService
    {
        private readonly AppDbContext _dbContext;
        private readonly ICacheProvider _cacheProvider;
        private readonly IOptions<UserCacheOptions> _cacheOptions;
        private readonly ILogger<UserService> _logger;

        public UserService(
            AppDbContext dbContext,
            ICacheProvider cacheProvider, 
            IOptions<UserCacheOptions> cacheOptions, 
            ILogger<UserService> logger)
        {
            _dbContext = dbContext;
            _cacheProvider = cacheProvider;
            _cacheOptions = cacheOptions;
            _logger = logger;
        }

        public async Task<User> CreateAsync(string email, string hashedPassword, Role? role)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(email);
            ArgumentException.ThrowIfNullOrWhiteSpace(hashedPassword);

            var user = new User()
            {
                Id = Guid.NewGuid(),
                Email = email,
                HashedPassword = hashedPassword,
                CreatedAt = DateTime.UtcNow
            };

            _ = _dbContext.Users.Add(user);

            if (role != null)
            {
                var userRole = new UserRole()
                {
                    UserId = user.Id,
                    RoleId = role.Id
                };

                _ = _dbContext.UserRoles.Add(userRole);
            }

            _ = await _dbContext.SaveChangesAsync();
            
            _cacheProvider.Remove(UserCacheKeys.ForEmail(email));
            return user;
        }

        public async Task<User> UpdateAsync(User user, string? newFirstName, string? newLastName)
        {
            ArgumentNullException.ThrowIfNull(user);

            _ = _dbContext.Users.Attach(user);

            user.FirstName = newFirstName;
            user.LastName = newLastName;

            _ = _dbContext.Users.Update(user);
            _ = await _dbContext.SaveChangesAsync();
            
            _cacheProvider.Remove([
                UserCacheKeys.ForId(user.Id),
                UserCacheKeys.ForEmail(user.Email)
            ]);

            return user;
        }

        public async Task<User?> GetAsync(Guid? id)
        {
            if (id == null || id == Guid.Empty)
            {
                return null;
            }
            
            try
            {
                var cacheKey = UserCacheKeys.ForId(id.Value);
                return await _cacheProvider.ReadThroughAsync(cacheKey, _cacheOptions.Value, async () =>
                {
                    return await _dbContext.Users
                        .AsNoTracking()
                        .FirstOrDefaultAsync(u => !u.IsDeleted && u.Id == id.Value);
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "User: {UserId} retrieval failed", id);
                return null;
            }
        }
        
        public async Task<User?> GetAsync(string? email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return null;
            }
            
            try
            {
                var cacheKey = UserCacheKeys.ForEmail(email);
                return await _cacheProvider.ReadThroughAsync(cacheKey, _cacheOptions.Value, async () =>
                {
                    return await _dbContext.Users
                        .AsNoTracking()
                        .FirstOrDefaultAsync(u => !u.IsDeleted && u.Email == email);
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "User: {UserEmail} retrieval failed", email);
                return null;
            }
        }

        public async Task<Guid> SoftDeleteAsync(User user)
        {
            ArgumentNullException.ThrowIfNull(user);

            var id = user.Id;
            var email = user.Email;
            
            _ = _dbContext.Users.Attach(user);
            
            user.FirstName = null;
            user.LastName = null;
            user.Email = Guid.NewGuid().ToString();
            user.HashedPassword = Guid.NewGuid().ToString();
            user.IsDeleted = true;
            user.DeletedAt = DateTime.UtcNow;
            
            _ = _dbContext.Users.Update(user);
            _ = await _dbContext.SaveChangesAsync();
            
            _cacheProvider.Remove([
                UserCacheKeys.ForId(id),
                UserCacheKeys.RolesForId(id),
                UserCacheKeys.ForEmail(email)
            ]);

            return id;
        }

        public async Task<List<UserRole>> ListRolesAsync(Guid? id)
        {
            if (id == null || id == Guid.Empty)
            {
                return [];
            }

            try
            {
                var cacheKey = UserCacheKeys.RolesForId(id.Value);
                return await _cacheProvider.ReadThroughAsync(cacheKey, _cacheOptions.Value, async () =>
                {
                    return await _dbContext.UserRoles
                        .AsNoTracking()
                        .Include(ur => ur.Role)
                        .Where(ur => ur.UserId == id.Value)
                        .ToListAsync();
                }) ?? [];
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "User: {UserId} roles retrieval failed", id);
                return [];
            }
        }
    }
}