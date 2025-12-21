using System;
using Api.Application.Behaviours;

namespace Api.Features.Projects.GetById
{
    public sealed record Query : IQuery<Response>
    {
        public Query(Guid? userId, Guid? projectId)
        {
            UserId = userId;
            ProjectId = projectId;
        }
        
        public Guid? UserId { get; init; }
        public Guid? ProjectId { get; init; }
    }
}