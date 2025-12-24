using System;
using Api.Application.Behaviours;
using Api.Domain.Enums;

namespace Api.Features.Roles.ListByScope
{
    public sealed record Query : IQuery<Response>
    {
        public Query(Guid? userId, RoleScope? scope)
        {
            UserId = userId ?? Guid.Empty;
            Scope = scope ?? RoleScope.Project;
        }
        
        public Guid UserId { get; init; }
        public RoleScope Scope { get; init; }
    }
}