using System;
using Api.Application.Behaviours;

namespace Api.Features.Users.RespondToMyInvitation
{
    public sealed record Command : ICommand<Response>
    {
        public Command(
            Guid? userId, 
            Guid? invitationId,
            string? status)
        {
            UserId = userId ?? Guid.Empty;
            InvitationId = invitationId ?? Guid.Empty;
            Status = status ?? string.Empty;
        }
        
        public Guid UserId { get; init; }
        public Guid InvitationId { get; init; }
        public string Status { get; init; }
    }
}