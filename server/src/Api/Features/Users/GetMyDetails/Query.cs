using System;
using Api.Application.Behaviours;

namespace Api.Features.Users.GetMyDetails
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