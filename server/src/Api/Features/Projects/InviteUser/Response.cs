using System;
using System.Text.Json.Serialization;
using Api.Domain.Constants;

namespace Api.Features.Projects.InviteUser
{
    public sealed record Response
    {
        public Response()
        {
        }
        
        [JsonPropertyName(FieldNames.NotificationId)]
        public Guid Id { get; init; }
    }
}