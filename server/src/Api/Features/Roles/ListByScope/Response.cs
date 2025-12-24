using System.Collections.Generic;
using System.Text.Json.Serialization;
using Api.Domain.Constants;

namespace Api.Features.Roles.ListByScope
{
    public sealed record Response
    {
        public Response()
        {
        }
        
        [JsonPropertyName(FieldNames.Roles)]
        [JsonPropertyOrder(0)]
        public List<RoleDto>? Roles { get; init; }
    }
}