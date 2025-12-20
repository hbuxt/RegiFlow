using System;
using Api.Application.Behaviours;

namespace Api.Features.Users.DeleteMyDetails
{
    public sealed record Command : ICommand
    {
        public Command(Guid? userId)
        {
            UserId = userId;
        }

        public Guid? UserId { get; init; }
    }
}