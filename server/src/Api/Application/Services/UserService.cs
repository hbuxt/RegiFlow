using System;
using System.Threading.Tasks;
using Api.Application.Abstractions;
using Api.Domain.Entities;
using Api.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Api.Application.Services
{
    internal sealed class UserService : IUserService
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<UserService> _logger;

        public UserService(
            AppDbContext dbContext,
            ILogger<UserService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<bool> ExistsAsync(Guid? id)
        {
            if (id == null || id == Guid.Empty)
            {
                return false;
            }

            return await _dbContext.Users
                .AsNoTracking()
                .AnyAsync(u => !u.IsDeleted && u.Id == id.Value);
        }
        
        public async Task<bool> ExistsAsync(string? email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }

            return await _dbContext.Users
                .AsNoTracking()
                .AnyAsync(u => !u.IsDeleted && u.Email == email);
        }

        public async Task<User?> GetAsync(Guid? id)
        {
            if (id == null || id == Guid.Empty)
            {
                return null;
            }
            
            try
            {
                return await _dbContext.Users
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => !u.IsDeleted && u.Id == id.Value);
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
                return await _dbContext.Users
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => !u.IsDeleted && u.Email == email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "User: {UserEmail} retrieval failed", email);
                return null;
            }
        }
    }
}