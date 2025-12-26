using System;
using System.Threading.Tasks;
using Api.Application.Abstractions;
using Api.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Api.Application.Services
{
    internal sealed class UserService : IUserService
    {
        private readonly AppDbContext _dbContext;

        public UserService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
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
    }
}