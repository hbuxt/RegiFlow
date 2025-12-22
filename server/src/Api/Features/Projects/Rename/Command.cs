using System;
using Api.Application.Behaviours;

namespace Api.Features.Projects.Rename
{
    public sealed record Command : ICommand<Response>
    {
        public Command(Guid? userId, Guid? projectId, string? name)
        {
            UserId = userId ?? Guid.Empty;
            ProjectId = projectId ?? Guid.Empty;
            Name = name?.Trim() ?? string.Empty;
        }
        
        public Guid UserId { get; init; }
        public Guid ProjectId { get; init; }
        public string Name { get; init; }
    }
}