using System;
using Api.Application.Behaviours;

namespace Api.Features.Users.Me.Roles
{
    public sealed record Query : IQuery<Response>
    {
        public Query(Guid? userId)
        {
            UserId = userId ?? Guid.Empty;
        }

        public Guid UserId { get; init; }
    }
}