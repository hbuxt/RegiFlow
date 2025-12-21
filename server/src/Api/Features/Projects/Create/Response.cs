using System;
using System.Text.Json.Serialization;
using Api.Domain.Constants;

namespace Api.Features.Projects.Create
{
    public sealed record Response
    {
        public Response()
        {
        }
        
        [JsonPropertyName(FieldNames.ProjectName)]
        [JsonPropertyOrder(0)]
        public string? Name { get; init; }
    }
}