using System;
using System.Threading.Tasks;
using Api.Domain.Entities;

namespace Api.Application.Abstractions
{
    public interface IUserService
    {
        Task<bool> ExistsAsync(Guid? id);
        Task<bool> ExistsAsync(string? email);
        Task<User?> GetAsync(string? email);
    }
}