using System;
using System.Threading.Tasks;
using Api.Application.Abstractions;
using Api.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Api.Application.Services
{
    internal sealed class ProjectService : IProjectService
    {
        private readonly AppDbContext _dbContext;

        public ProjectService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> ExistsAsync(Guid? id)
        {
            if (id == null || id == Guid.Empty)
            {
                return false;
            }

            return await _dbContext.Projects
                .AsNoTracking()
                .AnyAsync(p => p.Id == id);
        }
    }
}