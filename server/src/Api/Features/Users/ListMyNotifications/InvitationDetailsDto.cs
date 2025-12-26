using System;
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
        
        [JsonPropertyName(FieldNames.InvitationToken)]
        [JsonPropertyOrder(1)]
        public string? Token { get; init; }
        
        [JsonPropertyName(FieldNames.InvitationExpiresAt)]
        [JsonPropertyOrder(2)]
        public DateTime ExpiresAt { get; init; }
    }
}