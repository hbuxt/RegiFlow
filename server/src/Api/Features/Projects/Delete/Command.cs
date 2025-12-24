using System;
using Api.Application.Behaviours;

namespace Api.Features.Projects.Delete
{
    public sealed record Command : ICommand
    {
        public Command(Guid? userId, Guid? projectId)
        {
            UserId = userId ?? Guid.Empty;
            ProjectId = projectId ?? Guid.Empty;
        }
        
        public Guid UserId { get; init; }
        public Guid ProjectId { get; init; }
    }
}