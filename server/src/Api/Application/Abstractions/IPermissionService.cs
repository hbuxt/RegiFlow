using System;
using System.Threading.Tasks;

namespace Api.Application.Abstractions
{
    public interface IPermissionService
    {
        Task<bool> IsAuthorizedAsync(string permissionName, Guid userId);
        Task<bool> IsAuthorizedAsync(string permissionName, Guid userId, Guid projectId);
    }
}