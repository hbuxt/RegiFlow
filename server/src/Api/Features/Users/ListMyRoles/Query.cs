using System;
using Api.Application.Behaviours;

namespace Api.Features.Users.ListMyRoles
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