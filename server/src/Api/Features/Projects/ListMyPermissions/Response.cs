using System.Collections.Generic;
using System.Text.Json.Serialization;
using Api.Domain.Constants;

namespace Api.Features.Projects.ListMyPermissions
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