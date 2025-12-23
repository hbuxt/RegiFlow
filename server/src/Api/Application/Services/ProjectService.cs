using System;
using System.Threading.Tasks;
using Api.Application.Abstractions;
using Api.Domain.Entities;
using Api.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Api.Application.Services
{
    internal sealed class ProjectService : IProjectService
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<ProjectService> _logger;

        public ProjectService(
            AppDbContext dbContext, 
            ILogger<ProjectService> logger)
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

            try
            {
                return await _dbContext.Projects
                    .AsNoTracking()
                    .AnyAsync(p => p.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Project: {ProjectId} existence check failed", id);
                return false;
            }
        }

        public async Task<Project?> GetAsync(Guid? id)
        {
            if (id == null || id == Guid.Empty)
            {
                return null;
            }

            try
            {
                return await _dbContext.Projects
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p => p.Id == id.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Project: {ProjectId} retrieval failed", id);
                return null;
            }
        }
    }
}