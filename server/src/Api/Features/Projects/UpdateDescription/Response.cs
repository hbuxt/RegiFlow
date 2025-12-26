using System;
using System.Text.Json.Serialization;
using Api.Domain.Constants;

namespace Api.Features.Projects.UpdateDescription
{
    public sealed record Response
    {
        public Response()
        {
        }
        
        [JsonPropertyName(FieldNames.ProjectId)]
        [JsonPropertyOrder(0)]
        public Guid ProjectId { get; init; }
    }
}