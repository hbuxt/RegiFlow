using System;
using System.Threading.Tasks;
using Api.Domain.Entities;

namespace Api.Application.Abstractions
{
    public interface IProjectService
    {
        Task<bool> ExistsAsync(Guid? id);
    }
}