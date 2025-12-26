using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Api.Domain.Constants;

namespace Api.Features.Projects.InviteUser
{
    public sealed record Request
    {
        public Request()
        {
        }
        
        [JsonPropertyName(FieldNames.Email)]
        public string? Email { get; init; }
        
        [JsonPropertyName(FieldNames.Roles)]
        public List<Guid>? Roles { get; init; }
    }
}