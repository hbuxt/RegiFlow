using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Domain.Entities;

namespace Api.Application.Abstractions
{
    public interface IPermissionService
    {
        Task<List<Permission>> ListForUserAsync(Guid? id);
        Task<List<Permission>> ListForUserAsync(Guid? userId, Guid? projectId);
        Task<bool> IsAuthorizedAsync(string permissionName, Guid userId);
        Task<bool> IsAuthorizedAsync(string permissionName, Guid userId, Guid projectId);
    }
}