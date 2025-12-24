using System;
using Api.Application.Behaviours;

namespace Api.Features.Users.Me.Delete
{
    public sealed record Command : ICommand
    {
        public Command(Guid? userId)
        {
            UserId = userId ?? Guid.Empty;
        }

        public Guid UserId { get; init; }
    }
}