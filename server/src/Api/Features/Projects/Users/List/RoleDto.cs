using System;
using System.Text.Json.Serialization;
using Api.Domain.Constants;

namespace Api.Features.Projects.Users.List
{
    public sealed record RoleDto
    {
        public RoleDto()
        {
        }
        
        [JsonPropertyName(FieldNames.RoleId)]
        [JsonPropertyOrder(0)]
        public Guid Id { get; init; }
        
        [JsonPropertyName(FieldNames.RoleName)]
        [JsonPropertyOrder(1)]
        public string? Name { get; init; }
    }
}