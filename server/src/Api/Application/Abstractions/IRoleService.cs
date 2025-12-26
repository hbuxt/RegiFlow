using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Domain.Entities;
using Api.Domain.Enums;

namespace Api.Application.Abstractions
{
    public interface IRoleService
    {
        Task<List<Role>> GetAsync(List<Guid>? ids, RoleScope scope);
        Task<Role?> GetAsync(string? role, RoleScope scope);
    }
}