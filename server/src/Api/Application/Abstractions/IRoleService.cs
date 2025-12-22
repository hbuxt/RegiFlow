using System.Threading.Tasks;
using Api.Domain.Entities;
using Api.Domain.Enums;

namespace Api.Application.Abstractions
{
    public interface IRoleService
    {
        Task<Role?> GetAsync(string? role, RoleScope scope);
    }
}