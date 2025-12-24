using System.Collections.Generic;
using System.Text.Json.Serialization;
using Api.Domain.Constants;

namespace Api.Features.Users.Me.Permissions
{
    public sealed record Response
    {
        public Response()
        {
        }
        
        [JsonPropertyName(FieldNames.Permissions)]
        [JsonPropertyOrder(0)]
        public List<string>? Permissions { get; init; }
    }
}