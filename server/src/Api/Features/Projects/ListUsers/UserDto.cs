using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Api.Domain.Constants;

namespace Api.Features.Projects.ListUsers
{
    public sealed record UserDto
    {
        public UserDto()
        {
        }
        
        [JsonPropertyName(FieldNames.UserId)]
        [JsonPropertyOrder(0)]
        public Guid Id { get; init; }
        
        [JsonPropertyName(FieldNames.FirstName)]
        [JsonPropertyOrder(1)]
        public string? FirstName { get; init; }
        
        [JsonPropertyName(FieldNames.LastName)]
        [JsonPropertyOrder(2)]
        public string? LastName { get; init; }
        
        [JsonPropertyName(FieldNames.Email)]
        [JsonPropertyOrder(3)]
        public string? Email { get; init; }
        
        [JsonPropertyName(FieldNames.ProjectUserJoinedAt)]
        [JsonPropertyOrder(4)]
        public DateTime JoinedAt { get; init; }
        
        [JsonPropertyName(FieldNames.Roles)]
        [JsonPropertyOrder(5)]
        public List<RoleDto>? Roles { get; init; }
    }
}