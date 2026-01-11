using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Api.Domain.Constants;

namespace Api.Features.Users.ListMyNotifications
{
    public sealed record InvitationDetailsDto
    {
        public InvitationDetailsDto()
        {
        }
        
        [JsonPropertyName(FieldNames.InvitationRegarding)]
        [JsonPropertyOrder(0)]
        public ProjectDto? Regarding { get; init; }
        
        [JsonPropertyName(FieldNames.InvitationSentBy)]
        [JsonPropertyOrder(1)]
        public UserDto? SentBy { get; init; }
        
        [JsonPropertyName(FieldNames.InvitationRoles)]
        [JsonPropertyOrder(2)]
        public List<RoleDto>? Roles { get; init; }
        
        [JsonPropertyName(FieldNames.InvitationToken)]
        [JsonPropertyOrder(3)]
        public string? Token { get; init; }
        
        [JsonPropertyName(FieldNames.InvitationExpiresAt)]
        [JsonPropertyOrder(4)]
        public DateTime ExpiresAt { get; init; }
    }
}