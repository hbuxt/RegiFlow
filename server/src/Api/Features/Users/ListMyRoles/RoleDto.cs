using System;
using System.Text.Json.Serialization;
using Api.Domain.Constants;

namespace Api.Features.Users.ListMyRoles
{
    public sealed record RoleDto
    {
        public RoleDto()
        {
        }

        [JsonPropertyName(FieldNames.RoleId)]
        [JsonPropertyOrder(0)]
        public Guid Id { get; set; }

        [JsonPropertyName(FieldNames.RoleName)]
        [JsonPropertyOrder(1)]
        public string? Name { get; set; }
    }
}