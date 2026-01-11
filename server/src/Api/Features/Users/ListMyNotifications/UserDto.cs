using System;
using System.Text.Json.Serialization;
using Api.Domain.Constants;

namespace Api.Features.Users.ListMyNotifications
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
    }
}