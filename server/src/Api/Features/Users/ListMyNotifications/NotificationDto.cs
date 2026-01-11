using System;
using System.Text.Json.Serialization;
using Api.Domain.Constants;

namespace Api.Features.Users.ListMyNotifications
{
    public sealed record NotificationDto
    {
        public NotificationDto()
        {
        }
        
        [JsonPropertyName(FieldNames.NotificationId)]
        [JsonPropertyOrder(0)]
        public Guid Id { get; init; }
        
        [JsonPropertyName(FieldNames.NotificationType)]
        [JsonPropertyOrder(1)]
        public string? Type { get; init; }
        
        [JsonPropertyName(FieldNames.NotificationStatus)]
        [JsonPropertyOrder(2)]
        public string? Status { get; init; }
        
        [JsonPropertyName(FieldNames.Invitation)]
        [JsonPropertyOrder(4)]
        public InvitationDetailsDto? InvitationDetails { get; init; }
        
        [JsonPropertyName(FieldNames.NotificationCreatedAt)]
        [JsonPropertyOrder(5)]
        public DateTime CreatedAt { get; init; }
    }
}