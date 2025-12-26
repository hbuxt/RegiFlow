using System;
using System.Threading.Tasks;

namespace Api.Application.Abstractions
{
    public interface IUserService
    {
        Task<bool> ExistsAsync(Guid? id);
        Task<bool> ExistsAsync(string? email);
    }
}