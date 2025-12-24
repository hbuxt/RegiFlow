using System;
using Api.Application.Behaviours;

namespace Api.Features.Projects.Update
{
    public sealed record Command : ICommand<Response>
    {
        public Command(
            Guid? userId,
            Guid? projectId,
            string? description)
        {
            UserId = userId ?? Guid.Empty;
            ProjectId = projectId ?? Guid.Empty;
            Description = description?.Trim();
        }
        
        public Guid UserId { get; init; }
        public Guid ProjectId { get; init; }
        public string? Description { get; init; }
    }
}