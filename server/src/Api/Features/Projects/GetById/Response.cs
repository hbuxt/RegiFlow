using System;
using System.Text.Json.Serialization;
using Api.Domain.Constants;

namespace Api.Features.Projects.GetById
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
        
        [JsonPropertyName(FieldNames.ProjectDescription)]
        [JsonPropertyOrder(2)]
        public string? Description { get; init; }
        
        [JsonPropertyName(FieldNames.ProjectCreatedAt)]
        [JsonPropertyOrder(3)]
        public DateTime CreatedAt { get; init; }
    }
}