using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Domain.Entities;

namespace Api.Application.Abstractions
{
    public interface IProjectService
    {
        Task<Project> CreateAsync(User user, string name, string? description, Role? role);
        Task<Project> UpdateAsync(Project project, Guid userId, string? newDescription);
        Task<Project> RenameAsync(Project project, Guid userId, string newName);
        Task<Project?> GetAsync(Guid? id);
        Task<List<Project>> ListByCreatorAsync(Guid? id);
        Task<List<Project>> ListByUserAsync(Guid? id);
    }
}