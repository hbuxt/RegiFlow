using System;
using Api.Application.Behaviours;

namespace Api.Features.Users.ListMyProjects
{
    public sealed record Query : IQuery<Response>
    {
        public Query(Guid? userId)
        {
            UserId = userId;
        }
        
        public Guid? UserId { get; init; }
    }
}