using System;
using System.Collections.Generic;
using Api.Application.Behaviours;

namespace Api.Features.Projects.InviteUser
{
    public sealed record Command : ICommand
    {
        public Command(
            Guid? userId, 
            Guid? projectId, 
            string? email, 
            List<Guid>? roles)
        {
            UserId = userId ?? Guid.Empty;
            ProjectId = projectId ?? Guid.Empty;
            Email = email?.Trim() ?? string.Empty;
            Roles = roles ?? new List<Guid>();
        }
        
        public Guid UserId { get; init; }
        public Guid ProjectId { get; init; }
        public string Email { get; init; }
        public List<Guid> Roles { get; init; }
    }
}