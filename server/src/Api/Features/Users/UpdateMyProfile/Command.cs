using System;
using Api.Application.Behaviours;

namespace Api.Features.Users.UpdateMyProfile
{
    public sealed record Command : ICommand<Response>
    {
        public Command(
            Guid? userId,
            string? firstName,
            string? lastName)
        {
            UserId = userId ?? Guid.Empty;
            FirstName = firstName?.Trim();
            LastName = lastName?.Trim();
        }

        public Guid UserId { get; init; }
        public string? FirstName { get; init; }
        public string? LastName { get; init; }
    }
}