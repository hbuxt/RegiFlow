using System;
using System.Threading.Tasks;

namespace Api.Application.Abstractions
{
    public interface IProjectService
    {
        Task<bool> ExistsAsync(Guid? id);
    }
}