using System.Collections.Generic;
using System.Text.Json.Serialization;
using Api.Domain.Constants;

namespace Api.Features.Projects.ListUsers
{
    public sealed record Response
    {
        public Response()
        {
        }
        
        [JsonPropertyName(FieldNames.Users)]
        [JsonPropertyOrder(0)]
        public List<UserDto>? Users { get; init; }
    }
}