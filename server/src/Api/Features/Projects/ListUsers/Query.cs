using System;
using Api.Application.Behaviours;

namespace Api.Features.Projects.ListUsers
{
    public sealed record Query : IQuery<Response>
    {
        public Query(Guid? userId, Guid? projectId)
        {
            UserId = userId ?? Guid.Empty;
            ProjectId = projectId ?? Guid.Empty;
        }
        
        public Guid UserId { get; init; }
        public Guid ProjectId { get; init; }
    }
}