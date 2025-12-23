using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Application.Abstractions;
using Api.Domain.Entities;
using Api.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

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

        public async Task<Project> CreateAsync(User user, string name, string? description, Role? role)
        {
            ArgumentNullException.ThrowIfNull(user);
            ArgumentException.ThrowIfNullOrWhiteSpace(name);

            var project = new Project()
            {
                Id = Guid.NewGuid(),
                CreatedById = user.Id,
                Name = name,
                Description = description,
                CreatedAt = DateTime.UtcNow
            };

            _ = _dbContext.Projects.Add(project);

            var projectUser = new ProjectUser()
            {
                ProjectId = project.Id,
                UserId = user.Id,
                JoinedAt = DateTime.UtcNow
            };

            _ = _dbContext.ProjectUsers.Add(projectUser);
            
            if (role != null)
            {
                var projectOwner = new ProjectUserRole()
                {
                    Id = Guid.NewGuid(),
                    ProjectUserId = projectUser.Id,
                    RoleId = role.Id,
                    AssignedAt = DateTime.UtcNow
                };

                _ = _dbContext.ProjectUserRoles.Add(projectOwner);
            }

            _ = await _dbContext.SaveChangesAsync();

            return project;
        }

        public async Task<Project> UpdateAsync(Project project, Guid userId, string? newDescription)
        {
            ArgumentNullException.ThrowIfNull(project);
            ArgumentNullException.ThrowIfNull(userId);
            ArgumentException.ThrowIfNullOrWhiteSpace(newDescription);

            _ = _dbContext.Projects.Attach(project);

            project.Description = newDescription;

            _ = _dbContext.Projects.Update(project);
            _ = await _dbContext.SaveChangesAsync();
            
            return project;
        }

        public async Task<Project> RenameAsync(Project project, Guid userId, string newName)
        {
            ArgumentNullException.ThrowIfNull(project);
            ArgumentNullException.ThrowIfNull(userId);
            ArgumentException.ThrowIfNullOrWhiteSpace(newName);

            _ = _dbContext.Projects.Attach(project);

            project.Name = newName;

            _ = _dbContext.Projects.Update(project);
            _ = await _dbContext.SaveChangesAsync();
            
            return project;
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

        public async Task<List<Project>> ListByCreatorAsync(Guid? id)
        {
            if (id == null || id == Guid.Empty)
            {
                return [];
            }

            try
            {
                return await _dbContext.Projects
                    .AsNoTracking()
                    .Where(p => p.CreatedById == id.Value)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Projects by creator: {UserId} retrieval failed", id);
                return [];
            }
        }
        
        public async Task<List<Project>> ListByUserAsync(Guid? id)
        {
            if (id == null || id == Guid.Empty)
            {
                return [];
            }

            try
            {
                return await _dbContext.Projects
                    .AsNoTracking()
                    .Include(p => p.CreatedBy)
                    .Include(p => p.ProjectUsers)
                    .ThenInclude(pu => pu.User)
                    .Where(p => p.ProjectUsers.Any(pu => pu.UserId == id.Value))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Projects by user: {UserId} retrieval failed", id);
                return [];
            }
        }
    }
}