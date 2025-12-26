using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Api.Domain.Constants;

namespace Api.Features.Projects.Users.List
{
    public sealed record UserDto
    {
        public UserDto()
        {
        }
        
        [JsonPropertyName(FieldNames.UserId)]
        [JsonPropertyOrder(0)]
        public Guid Id { get; init; }
        
        [JsonPropertyName(FieldNames.Email)]
        [JsonPropertyOrder(1)]
        public string? Email { get; init; }
        
        [JsonPropertyName(FieldNames.Roles)]
        [JsonPropertyOrder(2)]
        public List<RoleDto>? Roles { get; init; }
    }
}