using System;
using System.Text.Json.Serialization;
using Api.Domain.Constants;

namespace Api.Features.Users.RespondToMyInvitation
{
    public sealed record Response
    {
        public Response()
        {
        }
        
        [JsonPropertyName(FieldNames.NotificationId)]
        [JsonPropertyOrder(0)]
        public Guid Id { get; init; }
    }
}