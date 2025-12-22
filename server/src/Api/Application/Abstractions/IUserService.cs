using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Domain.Entities;

namespace Api.Application.Abstractions
{
    public interface IUserService
    {
        Task<User> CreateAsync(string email, string hashedPassword, Role? role);
        Task<User> UpdateAsync(User user, string? newFirstName, string? newLastName);
        Task<User?> GetAsync(Guid? id);
        Task<User?> GetAsync(string? email);
        Task<Guid> SoftDeleteAsync(User user);
        Task<List<UserRole>> ListRolesAsync(Guid? id);
    }
}