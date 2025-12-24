using System.Collections.Generic;
using System.Text.Json.Serialization;
using Api.Domain.Constants;

namespace Api.Features.Users.Me.Projects
{
    public sealed record Response
    {
        public Response()
        {
        }
        
        [JsonPropertyName(FieldNames.Projects)]
        [JsonPropertyOrder(0)]
        public List<ProjectDto>? Projects { get; init; }
    }
}