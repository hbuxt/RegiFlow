using System;
using Api.Application.Behaviours;

namespace Api.Features.Projects.Create
{
    public sealed record Command : ICommand<Response>
    {
        public Command(
            Guid? userId, 
            string? name,
            string? description)
        {
            UserId = userId;
            Name = name?.Trim() ?? string.Empty;
            Description = description?.Trim();
        }
        
        public Guid? UserId { get; init; }
        public string Name { get; init; }
        public string? Description { get; init; }
    }
}