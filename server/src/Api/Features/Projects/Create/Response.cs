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
        
        [JsonPropertyName(FieldNames.ProjectId)]
        [JsonPropertyOrder(0)]
        public Guid Id { get; init; }
        
        [JsonPropertyName(FieldNames.ProjectName)]
        [JsonPropertyOrder(1)]
        public string? Name { get; init; }
    }
}