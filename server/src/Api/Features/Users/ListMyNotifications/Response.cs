using System.Collections.Generic;
using System.Text.Json.Serialization;
using Api.Domain.Constants;

namespace Api.Features.Users.ListMyNotifications
{
    public sealed record Response
    {
        public Response()
        {
        }
        
        [JsonPropertyName(FieldNames.Notifications)]
        [JsonPropertyOrder(0)]
        public List<NotificationDto>? Notifications { get; init; }
    }
}